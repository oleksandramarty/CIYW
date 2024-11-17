using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace Dictionaries.Domain.Models.Icons;

public class IconCategoryEntity : BaseIdEntity<int>, IActivatableEntity
{
    [Required] [MaxLength(100)] public required string Title { get; set; }
    public bool IsActive { get; set; }
    
    public ICollection<IconEntity> Icons { get; set; }
}