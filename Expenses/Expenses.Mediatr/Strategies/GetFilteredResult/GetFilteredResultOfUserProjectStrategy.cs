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
    GetFilteredResultOfUserProjectStrategy : GetFilteredResultStrategyResponseContext<GetFilteredUserProjectsRequest, UserProjectEntity, UserProjectResponse>, IGetFilteredResultStrategy<GetFilteredUserProjectsRequest, UserProjectResponse>
{
    private ICurrentUserRepository currentUserRepository;
    private readonly IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository;

    public GetFilteredResultOfUserProjectStrategy(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository
    ): base(mapper)
    {
        this.currentUserRepository = currentUserRepository;
        this.userProjectRepository = userProjectRepository;
    }

    public async Task<FilteredListResponse<UserProjectResponse>> GetFilteredResultAsync(
        GetFilteredUserProjectsRequest request, CancellationToken cancellationToken)
    {
        Guid? userId = await this.currentUserRepository.GetCurrentUserIdAsync();

        if (!userId.HasValue)
        {
            throw new EntityNotFoundException();
        }
        
        var query = this.userProjectRepository.GetQueryable(
            up => up.CreatedUserId == userId.Value,
            up => up.Include(up =>up.Balances));

        return await this.GetFilteredResultAsync(request, query, cancellationToken);
    }
}