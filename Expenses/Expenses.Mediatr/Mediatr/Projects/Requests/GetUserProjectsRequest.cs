using CommonModule.Shared.Responses.Expenses.Models.Projects;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Requests;

public class GetUserProjectsRequest: IRequest<List<UserProjectResponse>>
{
    
}