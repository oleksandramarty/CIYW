using CommonModule.Shared.Common;

namespace CommonModule.Shared.Responses.Categories;

public class UserCategoryResponse: BaseIdEntity<Guid>
{
    public int CategoryId { get; set; }
    public CategoryResponse Category { get; set; }
    
    public Guid UserId { get; set; }
}