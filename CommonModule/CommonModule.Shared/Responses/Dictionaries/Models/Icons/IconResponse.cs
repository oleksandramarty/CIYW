using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Icons;

public class IconResponse: BaseIdEntity<int>, IStatusEntity
{
    public string? Title { get; set; }
    public StatusEnum Status { get; set; }
    
    public int IconCategoryId { get; set; }
}