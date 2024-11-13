using Expenses.Domain.Models.Expenses;

namespace Expenses.Business;

public interface IBalanceRepository
{
    Task AddExpenseAsync(
        ExpenseEntity expenseEntity,
        CancellationToken cancellationToken);

    Task UpdateExpenseAsync(
        ExpenseEntity currentExpenseEntity,
        ExpenseEntity newExpenseEntity,
        CancellationToken cancellationToken);
    
    Task RemoveExpenseAsync(
        ExpenseEntity expenseEntity,
        CancellationToken cancellationToken);
}