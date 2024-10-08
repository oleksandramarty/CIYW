using CommonModule.Core.Extensions;
using CommonModule.Core.Strategies.GetFilteredResult;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class GetFilteredUserAllowedProjectsRequestHandler: IRequestHandler<GetFilteredUserAllowedProjectsRequest, FilteredListResponse<UserAllowedProjectResponse>>
{
    private readonly IGetFilteredResultStrategy<GetFilteredUserAllowedProjectsRequest, UserAllowedProjectResponse> strategy;

    public GetFilteredUserAllowedProjectsRequestHandler(
        IGetFilteredResultStrategy<GetFilteredUserAllowedProjectsRequest, UserAllowedProjectResponse> strategy
        )
    {
        this.strategy = strategy;
    }
    
    public async Task<FilteredListResponse<UserAllowedProjectResponse>> Handle(GetFilteredUserAllowedProjectsRequest request, CancellationToken cancellationToken)
    {
        request.CheckBaseFilter();

        return await this.strategy.GetFilteredResultAsync(request, cancellationToken);
    }
}