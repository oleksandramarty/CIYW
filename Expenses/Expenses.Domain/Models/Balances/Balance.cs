using CommonModule.Shared.Common;
using Expenses.Domain.Models.Projects;

namespace Expenses.Domain.Models.Balances;

public class Balance: BaseDateTimeEntity<Guid>
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public int CurrencyId { get; set; }
    
    public Guid UserProjectId { get; set; }
    public UserProject UserProject { get; set; }
}