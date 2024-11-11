using CommonModule.Shared.Common;
using CommonModule.Shared.Responses.Base;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Commands;

public class RemoveUserBalanceCommand: BaseIdEntity<Guid>, IRequest<BaseBoolResponse>
{
    
}