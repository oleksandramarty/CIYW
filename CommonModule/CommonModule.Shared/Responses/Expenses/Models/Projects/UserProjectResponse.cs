using CommonModule.Shared.Common;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;

namespace CommonModule.Shared.Responses.Expenses.Models.Projects;

public class UserProjectResponse: BaseDateTimeEntity<Guid>
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedUserId { get; set; }
    
    public ICollection<ExpenseResponse> Expenses { get; set; }
}