using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums.Expenses;

namespace Dictionaries.Domain.Models.Balances;

public class BalanceType : BaseIdEntity<int>, IActivatable
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public string Icon { get; set; }
    public BalanceEnum Type { get; set; }
}