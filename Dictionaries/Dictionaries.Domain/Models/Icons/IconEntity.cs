using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;
using Dictionaries.Domain.Models.Categories;

namespace Dictionaries.Domain.Models.Icons;

public class IconEntity : BaseIdEntity<int>, IStatusEntity
{
    [Required] [MaxLength(50)] public required string Title { get; set; }
    public StatusEnum Status { get; set; }

    public int IconCategoryId { get; set; }

    [JsonIgnore] public IconCategoryEntity? IconCategory { get; set; }

    [JsonIgnore] public ICollection<CategoryEntity> Categories { get; set; }
}