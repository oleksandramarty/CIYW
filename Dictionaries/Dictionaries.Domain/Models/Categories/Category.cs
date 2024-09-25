using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace Dictionaries.Domain.Models.Categories;

public class Category: BaseIdEntity<int>, ITreeEntity<int, int?>, IActivatable
{
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
    public bool IsActive { get; set; }

    public int? ParentId { get; set; }
    
    public bool IsPositive { get; set; }

    
    public ICollection<Category> Children { get; set; } = new List<Category>();
}