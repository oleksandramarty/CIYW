using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Responses.Expenses.Models.Balances;

public class BalanceResponse: BaseDateTimeEntity<Guid>, IBaseVersionEntity
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public int CurrencyId { get; set; }
    
    public Guid UserProjectId { get; set; }
    public string Version { get; set; }
}