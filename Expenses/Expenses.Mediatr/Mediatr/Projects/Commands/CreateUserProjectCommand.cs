using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Commands;

public class CreateUserProjectCommand: IRequest
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    
    public List<int> CurrencyIds { get; set; }
}