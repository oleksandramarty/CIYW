using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums.Expenses;
using Expenses.Domain.Models.Projects;

namespace Expenses.Domain.Models.Balances;

public class Balance: BaseDateTimeEntity<Guid>, IBaseVersionEntity, IActivatable
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public int CurrencyId { get; set; }
    public string Title { get; set; }
    public int IconId { get; set; }
    
    public Guid UserProjectId { get; set; }
    public UserProject UserProject { get; set; }
    public string Version { get; set; }
    
    public int BalanceTypeId { get; set; }
    public bool IsActive { get; set; }
}