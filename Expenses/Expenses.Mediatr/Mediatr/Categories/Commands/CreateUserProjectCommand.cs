using MediatR;

namespace Expenses.Mediatr.Mediatr.Categories.Commands;

public class CreateUserProjectCommand: IRequest
{
    public string Title { get; set; }
}