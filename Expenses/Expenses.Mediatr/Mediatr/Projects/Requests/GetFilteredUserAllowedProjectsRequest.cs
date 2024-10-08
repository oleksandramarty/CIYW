using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Requests;

public class GetFilteredUserAllowedProjectsRequest: BaseFilterRequest, IRequest<FilteredListResponse<UserAllowedProjectResponse>>
{
    
}