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

public class GetFilteredResultOfUserAllowedProjectStrategy: GetFilteredResultStrategyResponseContext<GetFilteredUserAllowedProjectsRequest, UserAllowedProjectEntity, UserAllowedProjectResponse>, IGetFilteredResultStrategy<GetFilteredUserAllowedProjectsRequest, UserAllowedProjectResponse>
{
    private readonly ICurrentUserRepository currentUserRepository;
    private readonly IReadGenericRepository<Guid, UserAllowedProjectEntity, ExpensesDataContext> userAllowedProjectRepository;

    public GetFilteredResultOfUserAllowedProjectStrategy(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IReadGenericRepository<Guid, UserAllowedProjectEntity, ExpensesDataContext> userAllowedProjectRepository
        ): base(mapper)
    {
        this.currentUserRepository = currentUserRepository;
        this.userAllowedProjectRepository = userAllowedProjectRepository;
    }

    public async Task<FilteredListResponse<UserAllowedProjectResponse>> GetFilteredResultAsync(GetFilteredUserAllowedProjectsRequest request, CancellationToken cancellationToken)
    {
        Guid? userId = await this.currentUserRepository.GetCurrentUserIdAsync();

        if (!userId.HasValue)
        {
            throw new EntityNotFoundException();
        }
        
        var query = this.userAllowedProjectRepository.GetQueryable(up => up.UserId  == userId.Value, 
            up => up.Include(p => p.UserProject).ThenInclude(b => b.Balances));
        
        return await this.GetFilteredResultAsync(request, query, cancellationToken);
    }
}