using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums.Expenses;

namespace Dictionaries.Domain.Models.Balances;

public class BalanceTypeEntity : BaseIdEntity<int>, IActivatableEntity
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public BalanceEnum Type { get; set; }
}