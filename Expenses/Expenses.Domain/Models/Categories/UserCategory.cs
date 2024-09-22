using CommonModule.Shared.Common;
using Expenses.Domain.Models.Expenses;

namespace Expenses.Domain.Models.Categories;

public class UserCategory: BaseIdEntity<Guid>
{
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public Guid UserId { get; set; }
    
    public ICollection<Expense> Expenses { get; set; }
}