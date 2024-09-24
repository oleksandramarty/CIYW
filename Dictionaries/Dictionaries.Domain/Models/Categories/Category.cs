using CommonModule.Shared.Common;

namespace Expenses.Domain.Models.Categories;

public class Category: BaseIdEntity<int>
{
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
    public bool IsActive { get; set; }
}