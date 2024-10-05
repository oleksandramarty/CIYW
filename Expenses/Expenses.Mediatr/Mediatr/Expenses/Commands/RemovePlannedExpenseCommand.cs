using CommonModule.Shared.Common;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Commands;

public class RemovePlannedExpenseCommand: BaseIdEntity<Guid>, IRequest<bool>
{
    
}