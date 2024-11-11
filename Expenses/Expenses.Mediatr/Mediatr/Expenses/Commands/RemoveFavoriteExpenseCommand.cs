using CommonModule.Shared.Common;
using CommonModule.Shared.Responses.Base;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Commands;

public class RemoveFavoriteExpenseCommand: BaseIdEntity<Guid>, IRequest<BaseBoolResponse>
{
    
}