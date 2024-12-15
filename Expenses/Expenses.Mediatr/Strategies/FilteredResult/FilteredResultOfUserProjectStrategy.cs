using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Strategies.FilteredResult;
using CommonModule.Interfaces;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Strategies.FilteredResult;

public class
    FilteredResultOfUserProjectStrategy : FilteredResultStrategyResponseContext<FilteredUserProjectsRequest, UserProjectEntity, UserProjectResponse>, IFilteredResultStrategy<FilteredUserProjectsRequest, UserProjectResponse>
{
    private readonly ICurrentUserRepository _currentUserRepository;
    private readonly IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> _readGenericUserProjectRepository;

    public FilteredResultOfUserProjectStrategy(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository
    ): base(mapper)
    {
        _currentUserRepository = currentUserRepository;
        _readGenericUserProjectRepository = readGenericUserProjectRepository;
    }

    public async Task<FilteredListResponse<UserProjectResponse>> FilteredResultAsync(
        FilteredUserProjectsRequest request, CancellationToken cancellationToken)
    {
        Guid? userId = await _currentUserRepository.CurrentUserIdAsync();

        if (!userId.HasValue)
        {
            throw new EntityNotFoundException();
        }
        
        var query = _readGenericUserProjectRepository.Queryable(
            up => up.CreatedUserId == userId.Value,
            up => up.Include(up =>up.Balances));

        return await FilteredResultAsync(request, query, cancellationToken);
    }
}