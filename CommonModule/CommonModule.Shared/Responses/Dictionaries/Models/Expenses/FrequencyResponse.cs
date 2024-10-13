using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums.Expenses;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Expenses;

public class FrequencyResponse: BaseIdEntity<int>, IActivatable
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public FrequencyEnum Type { get; set; }
}