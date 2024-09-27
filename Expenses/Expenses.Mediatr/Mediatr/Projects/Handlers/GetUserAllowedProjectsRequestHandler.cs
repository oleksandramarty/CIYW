using AutoMapper;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Domain;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class GetUserAllowedProjectsRequestHandler: MediatrAuthBase, IRequestHandler<GetUserAllowedProjectsRequest, List<UserAllowedProjectResponse>>
{
    private readonly IMapper mapper;
    private readonly IReadGenericRepository<Guid, UserAllowedProject, ExpensesDataContext> userAllowedProjectRepository;

    public GetUserAllowedProjectsRequestHandler(
        IAuthRepository authRepository,
        IMapper mapper,
        IReadGenericRepository<Guid, UserAllowedProject, ExpensesDataContext> userAllowedProjectRepository
        ) : base(authRepository)
    {
        this.userAllowedProjectRepository = userAllowedProjectRepository;
    }
    
    public async Task<List<UserAllowedProjectResponse>> Handle(GetUserAllowedProjectsRequest request, CancellationToken cancellationToken)
    {
        Guid userId = await GetCurrentUserIdAsync();
        
        var userAllowedProjects = await userAllowedProjectRepository
            .GetListAsync(
                x => x.UserId == userId,
                cancellationToken,
                up => up.Include(p => p.UserProject).ThenInclude(b => b.Balances));
        return userAllowedProjects.Select(up => this.mapper.Map<UserAllowedProject, UserAllowedProjectResponse>(up)).ToList();
    }
}