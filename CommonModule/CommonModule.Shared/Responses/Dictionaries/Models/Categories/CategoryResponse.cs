using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Categories;

public class CategoryResponse : BaseIdEntity<int>, ITreeEntity<int, int?>, ITreeChildren<CategoryResponse>, IActivatable
{
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
    public bool IsActive { get; set; }
    public bool IsPositive { get; set; }
    public int? ParentId { get; set; }

    public ICollection<CategoryResponse> Children { get; set; } = new List<CategoryResponse>();
}