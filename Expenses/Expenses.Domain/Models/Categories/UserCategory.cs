using CommonModule.Shared.Common;
using Expenses.Domain.Models.Expenses;

namespace Expenses.Domain.Models.Categories;

public class UserCategory: BaseDateTimeEntity<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
    
    public ICollection<Expense> Expenses { get; set; }
}