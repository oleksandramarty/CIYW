using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Strategies.FilteredResult;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Domain;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Strategies.FilteredResult;

public class FilteredResultOfUserAllowedProjectStrategy: FilteredResultStrategyResponseContext<FilteredUserAllowedProjectsRequest, UserAllowedProjectEntity, UserAllowedProjectResponse>, IFilteredResultStrategy<FilteredUserAllowedProjectsRequest, UserAllowedProjectResponse>
{
    private readonly ICurrentUserRepository currentUserRepository;
    private readonly IReadGenericRepository<Guid, UserAllowedProjectEntity, ExpensesDataContext> userAllowedProjectRepository;

    public FilteredResultOfUserAllowedProjectStrategy(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IReadGenericRepository<Guid, UserAllowedProjectEntity, ExpensesDataContext> userAllowedProjectRepository
        ): base(mapper)
    {
        this.currentUserRepository = currentUserRepository;
        this.userAllowedProjectRepository = userAllowedProjectRepository;
    }

    public async Task<FilteredListResponse<UserAllowedProjectResponse>> FilteredResultAsync(FilteredUserAllowedProjectsRequest request, CancellationToken cancellationToken)
    {
        Guid? userId = await this.currentUserRepository.CurrentUserIdAsync();

        if (!userId.HasValue)
        {
            throw new EntityNotFoundException();
        }
        
        var query = this.userAllowedProjectRepository.Queryable(up => up.UserId  == userId.Value, 
            up => up.Include(p => p.UserProject).ThenInclude(b => b.Balances));
        
        return await this.FilteredResultAsync(request, query, cancellationToken);
    }
}