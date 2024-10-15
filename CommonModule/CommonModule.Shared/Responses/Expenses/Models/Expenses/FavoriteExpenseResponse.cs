using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Responses.Expenses.Models.Expenses;

public class FavoriteExpenseResponse: BaseDateTimeEntity<Guid>, IBaseVersionEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Limit { get; set; }
    public decimal CurrentAmount { get; set; }
    public Guid BalanceId { get; set; }
    public int CategoryId { get; set; }
    public int FrequencyId { get; set; }
    
    public DateTime? EndDate { get; set; }
    public Guid UserProjectId { get; set; }
    public int IconId { get; set; }
    
    public Guid CreatedUserId { get; set; }
    public string Version { get; set; }
}