using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums.Expenses;

namespace Dictionaries.Domain.Models.Expenses;

public class FrequencyEntity: BaseIdEntity<int>, IActivatableEntity
{
    [Required] [MaxLength(20)] public required string Title { get; set; }
    [Required] [MaxLength(40)] public required string Description { get; set; }
    public bool IsActive { get; set; }
    public FrequencyEnum Type { get; set; }
}