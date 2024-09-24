using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Expenses.Models.Balances;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;

namespace CommonModule.Shared.Responses.Expenses.Models.Projects;

public class UserProjectResponse: BaseDateTimeEntity<Guid>, IActivatable
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedUserId { get; set; }
    
    public ICollection<BalanceResponse> Balances { get; set; }
    
    public ICollection<ExpenseResponse> Expenses { get; set; }
}