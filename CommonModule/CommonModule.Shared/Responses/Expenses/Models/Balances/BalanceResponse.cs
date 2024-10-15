using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums.Expenses;

namespace CommonModule.Shared.Responses.Expenses.Models.Balances;

public class BalanceResponse: BaseDateTimeEntity<Guid>, IBaseVersionEntity, IActivatable
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public int CurrencyId { get; set; }
    public string Title { get; set; }
    public int IconId { get; set; }
    
    public Guid UserProjectId { get; set; }
    public string Version { get; set; }
    
    public int BalanceTypeId { get; set; }
    public bool IsActive { get; set; }
}