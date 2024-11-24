using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Enums.Expenses;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Balances;

public class BalanceTypeResponse: BaseIdEntity<int>, IStatusEntity
{
    public string? Title { get; set; }
    public StatusEnum Status { get; set; }
    
    public BalanceEnum Type { get; set; }
}