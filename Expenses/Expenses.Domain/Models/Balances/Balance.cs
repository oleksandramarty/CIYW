using CommonModule.Shared.Common;

namespace Expenses.Domain.Models.Balances;

public class Balance: BaseDateTimeEntity<Guid>
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public int CurrencyId { get; set; }
}