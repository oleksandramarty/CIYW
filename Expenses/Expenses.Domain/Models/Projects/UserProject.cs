using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Expenses;

namespace Expenses.Domain.Models.Projects;

public class UserProject: BaseDateTimeEntity<Guid>, IActivatable, IBaseVersionEntity
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedUserId { get; set; }
    
    public ICollection<Balance> Balances { get; set; }
    
    public ICollection<UserAllowedProject> AllowedUsers { get; set; }
    
    public ICollection<Expense> Expenses { get; set; }
    public ICollection<PlannedExpense> PlannedExpenses { get; set; }
    public ICollection<FavoriteExpense> FavoriteExpenses { get; set; }
    public string Version { get; set; }
}