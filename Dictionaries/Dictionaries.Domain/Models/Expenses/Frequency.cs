using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums.Expenses;

namespace Dictionaries.Domain.Models.Expenses;

public class Frequency: BaseIdEntity<int>, IActivatable
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public FrequencyEnum FrequencyEnum { get; set; }
}