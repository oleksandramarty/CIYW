using CommonModule.Shared.Common;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Commands;

public class RemoveFavoriteExpenseCommand: BaseIdEntity<Guid>, IRequest<bool>
{
    
}