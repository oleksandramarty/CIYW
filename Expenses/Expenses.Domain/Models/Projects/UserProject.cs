using CommonModule.Shared.Common;
using Expenses.Domain.Models.Expenses;

namespace Expenses.Domain.Models.Projects;

public class UserProject: BaseDateTimeEntity<Guid>
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedUserId { get; set; }
    
    public ICollection<UserAllowedProject> AllowedUsers { get; set; }
    
    public ICollection<Expense> Expenses { get; set; }
}