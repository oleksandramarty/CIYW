using CommonModule.Shared.Common;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Commands;

public class RemoveUserBalanceCommand: BaseIdEntity<Guid>, IRequest<bool>
{
    
}