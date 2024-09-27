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
        Expense expense,
        CancellationToken cancellationToken)
    {
        using var transaction = await this.dataContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await this.UpdateBalanceAsync(expense.BalanceId, expense.CategoryId, expense.Amount, false, cancellationToken);
            
            await this.dataContext.Expenses.AddAsync(expense, cancellationToken);
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
        Expense currentExpense,
        Expense newExpense,
        CancellationToken cancellationToken)
    {
        using var transaction = await this.dataContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await this.UpdateBalanceAsync(currentExpense.BalanceId, currentExpense.CategoryId, currentExpense.Amount, true, cancellationToken);
            await this.UpdateBalanceAsync(newExpense.BalanceId, newExpense.CategoryId, newExpense.Amount, false, cancellationToken);
            
            currentExpense.Title = newExpense.Title;
            currentExpense.CategoryId = newExpense.CategoryId;
            currentExpense.Date = newExpense.Date;
            currentExpense.Description = newExpense.Description;
            currentExpense.Amount = newExpense.Amount;
            currentExpense.Modified = DateTime.UtcNow;
            
            this.dataContext.Expenses.Update(currentExpense);
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
        Expense expense,
        CancellationToken cancellationToken)
    {
        using var transaction = await this.dataContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await this.UpdateBalanceAsync(expense.BalanceId, expense.CategoryId, expense.Amount, true, cancellationToken);
            
            this.dataContext.Expenses.Remove(expense);
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
        Guid balanceId,
        int categoryId,
        decimal amount,
        bool isRefund,
        CancellationToken cancellationToken)
    {
        Balance? balance = await this.dataContext.Balances.FirstOrDefaultAsync(b => b.Id == balanceId, cancellationToken);
        this.entityValidator.ValidateExist(balance, balanceId);
        string currentCategory = await this.cacheBaseRepository.GetItemFromCacheAsync(CacheParams.DictionaryCategory, categoryId);
        this.entityValidator.ValidateExist(currentCategory);
        
        if (isRefund)
        {
            balance.Amount = currentCategory.Contains("\"IsPositive\":1") ? balance.Amount - amount : balance.Amount + amount;
        }
        else
        {
            balance.Amount = currentCategory.Contains("\"IsPositive\":1") ? balance.Amount + amount : balance.Amount - amount;
        }
        
        this.dataContext.Balances.Update(balance);
    }
}