using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Strategies.GetFilteredResult;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Domain;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Strategies.GetFilteredResult;

public class GetFilteredResultOfUserAllowedProjectStrategy: IGetFilteredResultStrategy<GetFilteredUserAllowedProjectsRequest, UserAllowedProjectResponse>
{
    private readonly IAuthRepository authRepository;
    private readonly IMapper mapper;
    private readonly IReadGenericRepository<Guid, UserAllowedProject, ExpensesDataContext> userAllowedProjectRepository;

    public GetFilteredResultOfUserAllowedProjectStrategy(
        IAuthRepository authRepository,
        IMapper mapper,
        IReadGenericRepository<Guid, UserAllowedProject, ExpensesDataContext> userAllowedProjectRepository
        )
    {
        this.authRepository = authRepository;
        this.mapper = mapper;
        this.userAllowedProjectRepository = userAllowedProjectRepository;
    }

    public async Task<FilteredListResponse<UserAllowedProjectResponse>> GetFilteredResultAsync(GetFilteredUserAllowedProjectsRequest request, CancellationToken cancellationToken)
    {
        Guid? userId = await this.authRepository.GetCurrentUserIdAsync();

        if (!userId.HasValue)
        {
            throw new EntityNotFoundException();
        }
        
        var query = this.userAllowedProjectRepository.GetQueryable(up => up.UserId  == userId.Value, 
            up => up.Include(p => p.UserProject).ThenInclude(b => b.Balances));
        
        var total = await query.CountAsync(cancellationToken);

        List<UserAllowedProject> userAllowedProjects = new List<UserAllowedProject>();

        if (request.Paginator != null)
        {
            userAllowedProjects = request.Paginator.IsFull ? 
                await query.ToListAsync(cancellationToken) :
                await query.Skip((request.Paginator.PageNumber - 1) * request.Paginator.PageSize)
                    .Take(request.Paginator.PageSize)
                    .ToListAsync(cancellationToken);
        }
        else
        {
            userAllowedProjects = await query.ToListAsync(cancellationToken);
        }

        return new FilteredListResponse<UserAllowedProjectResponse>
        {
            Entities = userAllowedProjects.Select(x => this.mapper.Map<UserAllowedProject, UserAllowedProjectResponse>(x)).ToList(),
            Paginator = request?.Paginator,
            TotalCount = total
        };
    }
}