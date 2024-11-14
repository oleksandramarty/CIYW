using AutoMapper;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Expenses;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Business;

public class BalanceRepository: IBalanceRepository
{
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly ExpensesDataContext dataContext;
    
    private readonly ICacheBaseRepository<int> cacheBaseRepository; 
    
    public BalanceRepository(
        IEntityValidator<ExpensesDataContext> entityValidator,
        ExpensesDataContext dataContext,
        ICacheBaseRepository<int> cacheBaseRepository
        )
    {
        this.entityValidator = entityValidator;
        this.dataContext = dataContext;
        this.cacheBaseRepository = cacheBaseRepository;
    }

    public async Task AddExpenseAsync(
        ExpenseEntity expenseEntity,
        CancellationToken cancellationToken)
    {
        using var transaction = await this.dataContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await this.UpdateBalanceAsync(expenseEntity, false, cancellationToken);
            
            await this.dataContext.Expenses.AddAsync(expenseEntity, cancellationToken);
            await this.dataContext.SaveChangesAsync(cancellationToken);

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
        using var transaction = await this.dataContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await this.UpdateBalanceAsync(currentExpenseEntity, true, cancellationToken);
            await this.UpdateBalanceAsync(newExpenseEntity, false, cancellationToken);
            
            currentExpenseEntity.Title = newExpenseEntity.Title;
            currentExpenseEntity.CategoryId = newExpenseEntity.CategoryId;
            currentExpenseEntity.Date = newExpenseEntity.Date;
            currentExpenseEntity.Description = newExpenseEntity.Description;
            currentExpenseEntity.Amount = newExpenseEntity.Amount;
            currentExpenseEntity.BalanceId = newExpenseEntity.BalanceId;
            
            this.dataContext.Expenses.Update(currentExpenseEntity);
            await this.dataContext.SaveChangesAsync(cancellationToken);

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
        using var transaction = await this.dataContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await this.UpdateBalanceAsync(expenseEntity, true, cancellationToken);
            
            this.dataContext.Expenses.Remove(expenseEntity);
            await this.dataContext.SaveChangesAsync(cancellationToken);

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
        BalanceEntity? balance = await this.dataContext.Balances.FirstOrDefaultAsync(b => b.Id == expenseEntity.BalanceId, cancellationToken);
        this.entityValidator.IsEntityExist(balance);
        string currentCategory = await this.cacheBaseRepository.GetItemFromCacheAsync(CacheParams.DictionaryCategory, expenseEntity.CategoryId);
        this.entityValidator.IsEntityExist(currentCategory);
        
        FavoriteExpenseEntity? favoriteExpense = expenseEntity.FavoriteExpenseId.HasValue ?
            await this.dataContext.FavoriteExpenses
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
            this.dataContext.FavoriteExpenses.Update(favoriteExpense);
        }
        
        this.dataContext.Balances.Update(balance);
    }
}