using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Commands;

public class CreateUserProjectCommand: IRequest
{
    public string Title { get; set; }
    
    public int CurrencyId { get; set; }
}