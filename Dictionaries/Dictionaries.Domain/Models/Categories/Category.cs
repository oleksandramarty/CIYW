using System.Text.Json.Serialization;
using CommonModule.Core.JsonConverter;
using CommonModule.Core.Utils;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace Dictionaries.Domain.Models.Categories;

public class Category: BaseIdEntity<int>, ITreeEntity<int, int?>, IActivatable
{
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
    [JsonConverter(typeof(BooleanJsonConverter))]
    public bool IsActive { get; set; }

    public int? ParentId { get; set; }
    [JsonConverter(typeof(BooleanJsonConverter))]
    public bool IsPositive { get; set; }

    [JsonIgnore]
    public ICollection<Category> Children { get; set; } = new List<Category>();
}