using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using Dictionaries.Domain.Models.Categories;

namespace Dictionaries.Domain.Models.Icons;

public class Icon : BaseIdEntity<int>, IActivatable
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    
    public int IconCategoryId { get; set; }
    
    [JsonIgnore]
    public IconCategory IconCategory { get; set; }
    
    [JsonIgnore]
    public ICollection<Category> Categories { get; set; }
}