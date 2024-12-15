using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Expenses;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Business;

public class BalanceRepository: IBalanceRepository
{
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly ExpensesDataContext _dataContext;
    
    private readonly ICacheBaseRepository<int> _cacheBaseRepository; 
    
    public BalanceRepository(
        IEntityValidator<ExpensesDataContext> entityValidator,
        ExpensesDataContext dataContext,
        ICacheBaseRepository<int> cacheBaseRepository
        )
    {
        _entityValidator = entityValidator;
        _dataContext = dataContext;
        _cacheBaseRepository = cacheBaseRepository;
    }

    public async Task AddExpenseAsync(
        ExpenseEntity expenseEntity,
        CancellationToken cancellationToken)
    {
        using var transaction = await _dataContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await UpdateBalanceAsync(expenseEntity, false, cancellationToken);
            
            await _dataContext.Expenses.AddAsync(expenseEntity, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    
    public async Task UpdateExpenseAsync(
        ExpenseEntity currentExpenseEntity,
        ExpenseEntity newExpenseEntity,
        CancellationToken cancellationToken)
    {
        using var transaction = await _dataContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await UpdateBalanceAsync(currentExpenseEntity, true, cancellationToken);
            await UpdateBalanceAsync(newExpenseEntity, false, cancellationToken);
            
            currentExpenseEntity.Title = newExpenseEntity.Title;
            currentExpenseEntity.CategoryId = newExpenseEntity.CategoryId;
            currentExpenseEntity.Date = newExpenseEntity.Date;
            currentExpenseEntity.Description = newExpenseEntity.Description;
            currentExpenseEntity.Amount = newExpenseEntity.Amount;
            currentExpenseEntity.BalanceId = newExpenseEntity.BalanceId;
            
            _dataContext.Expenses.Update(currentExpenseEntity);
            await _dataContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task RemoveExpenseAsync(
        ExpenseEntity expenseEntity,
        CancellationToken cancellationToken)
    {
        using var transaction = await _dataContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await UpdateBalanceAsync(expenseEntity, true, cancellationToken);
            
            _dataContext.Expenses.Remove(expenseEntity);
            await _dataContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task UpdateBalanceAsync(
        ExpenseEntity expenseEntity,
        bool isRefund,
        CancellationToken cancellationToken)
    {
        BalanceEntity? balance = await _dataContext.Balances.FirstOrDefaultAsync(b => b.Id == expenseEntity.BalanceId, cancellationToken);
        if (balance == null)
        {
            throw new EntityNotFoundException();
        }
        
        string? currentCategory = await _cacheBaseRepository.ItemFromCacheAsync(CacheParams.DictionaryCategory, expenseEntity.CategoryId);
        if (string.IsNullOrEmpty(currentCategory))
        {
            throw new EntityNotFoundException();
        }
        
        FavoriteExpenseEntity? favoriteExpense = expenseEntity.FavoriteExpenseId.HasValue ?
            await _dataContext.FavoriteExpenses
                .FirstOrDefaultAsync(fe => fe.Id == expenseEntity.FavoriteExpenseId, cancellationToken) :
            null;

        if (favoriteExpense != null && favoriteExpense.CurrentAmount == null)
        {
            favoriteExpense.CurrentAmount = 0.0m;
        }
        
        if (isRefund)
        {
            bool isNegative = currentCategory.ToLower().Contains("\"ispositive\":1");
            balance.Amount = isNegative ? balance.Amount - expenseEntity.Amount : balance.Amount + expenseEntity.Amount;
            if (favoriteExpense != null)
            {
                favoriteExpense.CurrentAmount -= expenseEntity.Amount;
            }
        }
        else
        {
            bool isPositive = currentCategory.ToLower().Contains("\"ispositive\":true");
            balance.Amount = isPositive ? balance.Amount + expenseEntity.Amount : balance.Amount - expenseEntity.Amount;
            if (favoriteExpense != null)
            {
                favoriteExpense.CurrentAmount += expenseEntity.Amount;
            }
        }
        
        if (favoriteExpense != null)
        {
            _dataContext.FavoriteExpenses.Update(favoriteExpense);
        }
        
        _dataContext.Balances.Update(balance);
    }
}