using CommonModule.Shared.Common;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Requests;

public class GetUserProjectByIdRequest: BaseIdEntity<Guid>, IRequest<UserProjectResponse>
{
    
}