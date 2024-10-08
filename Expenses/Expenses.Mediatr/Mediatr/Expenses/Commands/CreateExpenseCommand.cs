using CommonModule.Shared.Common;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Commands;

public class CreateExpenseCommand: IRequest
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public int CategoryId { get; set; }
    public Guid UserProjectId { get; set; }
    public Guid BalanceId { get; set; }
}