using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;
using CommonModule.Shared.JsonConvertors;
using Dictionaries.Domain.Models.Icons;

namespace Dictionaries.Domain.Models.Categories;

public class CategoryEntity : BaseIdEntity<int>, ITreeEntityEntity<int, int?>, IStatusEntity
{
    [Required] [MaxLength(70)] public required string Title { get; set; }
    public int IconId { get; set; }
    [JsonIgnore]
    public IconEntity? Icon { get; set; }
    [Required] [MaxLength(7)] public required string Color { get; set; }
    public StatusEnum Status { get; set; }

    public int? ParentId { get; set; }
    
    [JsonConverter(typeof(BooleanJsonConverter))]
    public bool IsPositive { get; set; }

    [JsonIgnore]
    public ICollection<CategoryEntity> Children { get; set; }
}