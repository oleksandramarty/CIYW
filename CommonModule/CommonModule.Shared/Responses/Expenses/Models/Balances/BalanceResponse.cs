using CommonModule.Shared.Common;

namespace CommonModule.Shared.Responses.Expenses.Models.Balances;

public class BalanceResponse: BaseDateTimeEntity<Guid>
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public int CurrencyId { get; set; }
    
    public Guid UserProjectId { get; set; }
}