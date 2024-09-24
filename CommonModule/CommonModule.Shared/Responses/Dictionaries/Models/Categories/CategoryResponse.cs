using CommonModule.Shared.Common;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Categories;

public class CategoryResponse: BaseIdEntity<int>
{
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
    public bool IsActive { get; set; }
}