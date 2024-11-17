using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Categories;

public class CategoryResponse : BaseIdEntity<int>, ITreeEntityEntity<int, int?>, ITreeChildrenEntity<CategoryResponse>, IActivatableEntity
{
    public string? Title { get; set; }
    public int IconId { get; set; }
    public string? Color { get; set; }
    public bool IsActive { get; set; }
    public bool IsPositive { get; set; }
    public int? ParentId { get; set; }

    public ICollection<CategoryResponse> Children { get; set; }
}