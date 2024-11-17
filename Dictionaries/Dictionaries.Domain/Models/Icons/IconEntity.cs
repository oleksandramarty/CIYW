using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using Dictionaries.Domain.Models.Categories;

namespace Dictionaries.Domain.Models.Icons;

public class IconEntity : BaseIdEntity<int>, IActivatableEntity
{
    [Required] [MaxLength(50)] public required string Title { get; set; }
    public bool IsActive { get; set; }
    
    public int IconCategoryId { get; set; }
    
    [JsonIgnore]
    public IconCategoryEntity? IconCategory { get; set; }
    
    [JsonIgnore]
    public ICollection<CategoryEntity> Categories { get; set; }
}