using CommonModule.Shared.Common.BaseInterfaces;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Commands;

public class UpdateUserProjectCommand: IBaseIdEntity<Guid>, IRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
}