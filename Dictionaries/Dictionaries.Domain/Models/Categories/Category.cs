using System.Text.Json.Serialization;
using CommonModule.Core.JsonConverter;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.JsonConvertors;
using Dictionaries.Domain.Models.Icons;

namespace Dictionaries.Domain.Models.Categories;

public class Category : BaseIdEntity<int>, ITreeEntity<int, int?>, IActivatable
{
    public string Title { get; set; }
    public int IconId { get; set; }
    [JsonIgnore]
    public Icon Icon { get; set; }
    public string Color { get; set; }
    
    public bool IsActive { get; set; }

    public int? ParentId { get; set; }
    
    [JsonConverter(typeof(BooleanJsonConverter))]
    public bool IsPositive { get; set; }

    [JsonIgnore]
    public ICollection<Category> Children { get; set; } = new List<Category>();
}