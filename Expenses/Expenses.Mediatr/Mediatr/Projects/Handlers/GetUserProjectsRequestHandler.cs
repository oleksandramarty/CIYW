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

public class GetUserProjectsRequestHandler: MediatrAuthBase, IRequestHandler<GetUserProjectsRequest, List<UserProjectResponse>>
{
    private readonly IMapper mapper;
    private readonly IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;
    
    public GetUserProjectsRequestHandler(
        IAuthRepository authRepository,
        IMapper mapper,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
        ) : base(authRepository)
    {
        this.userProjectRepository = userProjectRepository;
    }
    
    public async Task<List<UserProjectResponse>> Handle(GetUserProjectsRequest request, CancellationToken cancellationToken)
    {
        var userId = await GetUserIdAsync();
        
        var userProjects = await userProjectRepository
            .GetListAsync(
                x => x.CreatedUserId == userId,
                cancellationToken,
                up => up.Include(b => b.Balances));
        return userProjects.Select(up => this.mapper.Map<UserProject, UserProjectResponse>(up)).ToList();
    }
}