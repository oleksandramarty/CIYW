using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Icons;

public class IconCategoryResponse: BaseIdEntity<int>, IStatusEntity
{
    public string? Title { get; set; }
    public StatusEnum Status { get; set; }
    
    public ICollection<IconResponse> Icons { get; set; }
}