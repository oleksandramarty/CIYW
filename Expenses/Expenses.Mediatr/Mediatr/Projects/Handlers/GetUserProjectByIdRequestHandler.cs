using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Domain;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class GetUserProjectByIdRequestHandler: MediatrAuthBase, IRequestHandler<GetUserProjectByIdRequest, UserProjectResponse>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;
    
    public GetUserProjectByIdRequestHandler(
        IAuthRepository authRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
        ): base(authRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.userProjectRepository = userProjectRepository;
    }
    
    public async Task<UserProjectResponse> Handle(GetUserProjectByIdRequest command, CancellationToken cancellationToken)
    {
        Guid? userId = await this.GetCurrentUserIdAsync();
        this.entityValidator.ValidateExist(userId);
        
        UserProject userProject = await this.userProjectRepository
            .GetAsync(
                up => up.Id == command.Id, 
                cancellationToken, 
                up => up.Include(up => up.AllowedUsers).Include(up => up.Balances));
        this.entityValidator.ValidateExist<UserProject, Guid>(userProject, command.Id);
        
        if (userProject.CreatedUserId != userId && userProject.AllowedUsers.All(au => au.UserId != userId))
        {
            throw new ForbiddenException();
        }
        
        return mapper.Map<UserProjectResponse>(userProject);
    }
}