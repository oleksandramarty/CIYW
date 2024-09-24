using CommonModule.Shared.Common;

namespace CommonModule.Shared.Responses.Expenses.Models.Categories;

public class UserCategoryResponse: BaseDateTimeEntity<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
}