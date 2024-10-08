using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Responses.Expenses.Models.Expenses;

public class PlannedExpenseResponse: BaseDateTimeEntity<Guid>, IActivatable, IBaseVersionEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
    public Guid BalanceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime NextDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid UserProjectId { get; set; }
    
    public int FrequencyId { get; set; }
    
    public bool IsActive { get; set; }
    public string Version { get; set; }
}