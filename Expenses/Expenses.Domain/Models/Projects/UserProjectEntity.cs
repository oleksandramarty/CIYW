using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Expenses;

namespace Expenses.Domain.Models.Projects;

public class UserProjectEntity: BaseDateTimeEntity<Guid>, IActivatableEntity, IBaseVersionEntity
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedUserId { get; set; }
    
    public ICollection<BalanceEntity> Balances { get; set; }
    
    public ICollection<UserAllowedProjectEntity> AllowedUsers { get; set; }
    
    public ICollection<ExpenseEntity> Expenses { get; set; }
    public ICollection<PlannedExpenseEntity> PlannedExpenses { get; set; }
    public ICollection<FavoriteExpenseEntity> FavoriteExpenses { get; set; }
    public string Version { get; set; }
}