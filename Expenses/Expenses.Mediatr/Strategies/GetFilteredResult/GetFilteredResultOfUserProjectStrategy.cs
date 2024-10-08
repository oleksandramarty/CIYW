using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Strategies.GetFilteredResult;
using CommonModule.Interfaces;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Strategies.GetFilteredResult;

public class
    GetFilteredResultOfUserProjectStrategy : IGetFilteredResultStrategy<GetFilteredUserProjectsRequest, UserProjectResponse>
{
    private IAuthRepository authRepository;
    private readonly IMapper mapper;
    private readonly IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;

    public GetFilteredResultOfUserProjectStrategy(
        IAuthRepository authRepository,
        IMapper mapper,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
    )
    {
        this.authRepository = authRepository;
        this.mapper = mapper;
        this.userProjectRepository = userProjectRepository;
    }

    public async Task<FilteredListResponse<UserProjectResponse>> GetFilteredResultAsync(
        GetFilteredUserProjectsRequest request, CancellationToken cancellationToken)
    {
        Guid? userId = await this.authRepository.GetCurrentUserIdAsync();

        if (!userId.HasValue)
        {
            throw new EntityNotFoundException();
        }
        
        var query = this.userProjectRepository.GetQueryable(
            up => up.CreatedUserId == userId.Value,
            up => up.Include(up =>up.Balances));

        var total = await query.CountAsync(cancellationToken);

        List<UserProject> userProjects = new List<UserProject>();

        if (request.Paginator != null)
        {
            userProjects = request.Paginator.IsFull ? 
                await query.ToListAsync(cancellationToken) :
                await query.Skip((request.Paginator.PageNumber - 1) * request.Paginator.PageSize)
                .Take(request.Paginator.PageSize)
                .ToListAsync(cancellationToken);
        }
        else
        {
            userProjects = await query.ToListAsync(cancellationToken);
        }

        return new FilteredListResponse<UserProjectResponse>
        {
            Entities = userProjects.Select(x => this.mapper.Map<UserProject, UserProjectResponse>(x)).ToList(),
            Paginator = request?.Paginator,
            TotalCount = total
        };
    }
}