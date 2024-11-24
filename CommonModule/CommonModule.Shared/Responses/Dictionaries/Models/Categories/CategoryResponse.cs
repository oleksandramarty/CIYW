using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Categories;

public class CategoryResponse : BaseIdEntity<int>, ITreeEntityEntity<int, int?>, ITreeChildrenEntity<CategoryResponse>, IStatusEntity
{
    public string? Title { get; set; }
    public int IconId { get; set; }
    public string? Color { get; set; }
    public StatusEnum Status { get; set; }
    public bool IsPositive { get; set; }
    public int? ParentId { get; set; }

    public ICollection<CategoryResponse> Children { get; set; }
}