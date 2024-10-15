using CommonModule.Shared.Common;
using CommonModule.Shared.Enums.Expenses;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Commands;

public class UpdateUserBalanceCommand: BaseIdEntity<Guid>, IRequest
{
    public int CurrencyId { get; set; }
    public string Title { get; set; }
    public int IconId { get; set; }
    public int BalanceTypeId { get; set; }
    public bool IsActive { get; set; }
    public Guid UserProjectId { get; set; }
}