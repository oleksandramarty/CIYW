using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Commands;

public class CreatePlannedExpenseCommand: IRequest<BaseEntityIdResponse<Guid>>
{
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