using Expenses.Domain.Models.Expenses;

namespace Expenses.Business;

public interface IBalanceRepository
{
    Task AddExpenseAsync(
        Expense expense,
        CancellationToken cancellationToken);

    Task UpdateExpenseAsync(
        Expense currentExpense,
        Expense newExpense,
        CancellationToken cancellationToken);
    
    Task RemoveExpenseAsync(
        Expense expense,
        CancellationToken cancellationToken);
}