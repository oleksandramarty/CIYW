using AutoMapper;
using CommonModule.Core.Extensions;
using CommonModule.Core.Mediatr;
using CommonModule.Core.Strategies.GetFilteredResult;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Domain;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class GetFilteredUserProjectsRequestHandler: IRequestHandler<GetFilteredUserProjectsRequest, FilteredListResponse<UserProjectResponse>>
{
    private readonly IGetFilteredResultStrategy<GetFilteredUserProjectsRequest, UserProjectResponse> strategy;
    
    public GetFilteredUserProjectsRequestHandler(
        IGetFilteredResultStrategy<GetFilteredUserProjectsRequest, UserProjectResponse> strategy
        )
    {
        this.strategy = strategy;
    }
    
    public async Task<FilteredListResponse<UserProjectResponse>> Handle(GetFilteredUserProjectsRequest request, CancellationToken cancellationToken)
    {
        
        request.CheckBaseFilter();

        return await this.strategy.GetFilteredResultAsync(request, cancellationToken);
    }
}