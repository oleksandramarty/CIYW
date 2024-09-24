using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Categories;

public class CategoryResponse: BaseIdEntity<int>, IActivatable
{
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
    public bool IsActive { get; set; }
}