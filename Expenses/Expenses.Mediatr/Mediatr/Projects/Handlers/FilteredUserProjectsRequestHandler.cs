using AutoMapper;
using CommonModule.Core.Extensions;
using CommonModule.Core.Mediatr;
using CommonModule.Core.Strategies.FilteredResult;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Domain;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class FilteredUserProjectsRequestHandler: IRequestHandler<FilteredUserProjectsRequest, FilteredListResponse<UserProjectResponse>>
{
    private readonly IFilteredResultStrategy<FilteredUserProjectsRequest, UserProjectResponse> strategy;
    
    public FilteredUserProjectsRequestHandler(
        IFilteredResultStrategy<FilteredUserProjectsRequest, UserProjectResponse> strategy
        )
    {
        this.strategy = strategy;
    }
    
    public async Task<FilteredListResponse<UserProjectResponse>> Handle(FilteredUserProjectsRequest request, CancellationToken cancellationToken)
    {
        
        request.CheckBaseFilter();

        return await this.strategy.FilteredResultAsync(request, cancellationToken);
    }
}