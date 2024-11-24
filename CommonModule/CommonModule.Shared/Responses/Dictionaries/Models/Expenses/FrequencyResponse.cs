using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Enums.Expenses;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Expenses;

public class FrequencyResponse: BaseIdEntity<int>, IStatusEntity
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public StatusEnum Status { get; set; }
    public FrequencyEnum Type { get; set; }
}