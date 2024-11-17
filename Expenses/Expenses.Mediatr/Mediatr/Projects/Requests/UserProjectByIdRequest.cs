using CommonModule.Shared.Common;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Requests;

public class UserProjectByIdRequest: BaseIdEntity<Guid>, IRequest<UserProjectResponse>
{
    public UserProjectByIdRequest()
    {
        
    }

    public UserProjectByIdRequest(Guid id)
    {
        Id = id;
    }
}