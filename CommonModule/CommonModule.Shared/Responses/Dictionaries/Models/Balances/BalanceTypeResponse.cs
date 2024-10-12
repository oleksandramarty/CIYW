using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums.Expenses;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Balances;

public class BalanceTypeResponse: BaseIdEntity<int>, IActivatable
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public string Icon { get; set; }
    
    public BalanceEnum Type { get; set; }
}