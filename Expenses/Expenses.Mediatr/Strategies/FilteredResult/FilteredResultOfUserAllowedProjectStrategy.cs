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
    private readonly ICurrentUserRepository _currentUserRepository;
    private readonly IReadGenericRepository<Guid, UserAllowedProjectEntity, ExpensesDataContext> _readGenericUserAllowedProjectRepository;

    public FilteredResultOfUserAllowedProjectStrategy(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IReadGenericRepository<Guid, UserAllowedProjectEntity, ExpensesDataContext> readGenericUserAllowedProjectRepository
        ): base(mapper)
    {
        _currentUserRepository = currentUserRepository;
        _readGenericUserAllowedProjectRepository = readGenericUserAllowedProjectRepository;
    }

    public async Task<FilteredListResponse<UserAllowedProjectResponse>> FilteredResultAsync(FilteredUserAllowedProjectsRequest request, CancellationToken cancellationToken)
    {
        Guid? userId = await _currentUserRepository.CurrentUserIdAsync();

        if (!userId.HasValue)
        {
            throw new EntityNotFoundException();
        }
        
        var query = _readGenericUserAllowedProjectRepository.Queryable(up => up.UserId  == userId.Value, 
            up => up.Include(p => p.UserProject).ThenInclude(b => b.Balances));
        
        return await FilteredResultAsync(request, query, cancellationToken);
    }
}