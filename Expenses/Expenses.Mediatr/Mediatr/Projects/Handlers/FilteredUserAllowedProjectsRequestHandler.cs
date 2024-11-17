using CommonModule.Core.Extensions;
using CommonModule.Core.Strategies.FilteredResult;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class FilteredUserAllowedProjectsRequestHandler: IRequestHandler<FilteredUserAllowedProjectsRequest, FilteredListResponse<UserAllowedProjectResponse>>
{
    private readonly IFilteredResultStrategy<FilteredUserAllowedProjectsRequest, UserAllowedProjectResponse> strategy;

    public FilteredUserAllowedProjectsRequestHandler(
        IFilteredResultStrategy<FilteredUserAllowedProjectsRequest, UserAllowedProjectResponse> strategy
        )
    {
        this.strategy = strategy;
    }
    
    public async Task<FilteredListResponse<UserAllowedProjectResponse>> Handle(FilteredUserAllowedProjectsRequest request, CancellationToken cancellationToken)
    {
        request.CheckBaseFilter();

        return await this.strategy.FilteredResultAsync(request, cancellationToken);
    }
}