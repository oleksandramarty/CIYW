using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Commands;

public class CreateOrUpdatePlannedExpenseCommand: IRequest
{
    public Guid? Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
    public Guid BalanceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public Guid UserProjectId { get; set; }
    
    public int FrequencyId { get; set; }
    
    public bool IsActive { get; set; }
}