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

public class GetUserProjectByIdRequestHandler: MediatrExpensesBase, IRequestHandler<GetUserProjectByIdRequest, UserProjectResponse>
{
    private readonly IMapper mapper;
    
    public GetUserProjectByIdRequestHandler(
        IAuthRepository authRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
        ): base(authRepository, entityValidator, userProjectRepository)
    {
        this.mapper = mapper;
    }
    
    public async Task<UserProjectResponse> Handle(GetUserProjectByIdRequest command, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetCurrentUserIdAsync();
        UserProject userProject = await this.GetUserProjectByIdAsync(command.Id, cancellationToken);
        
        if (userProject.CreatedUserId != userId && userProject.AllowedUsers.All(au => au.UserId != userId))
        {
            throw new ForbiddenException();
        }
        
        return mapper.Map<UserProjectResponse>(userProject);
    }
}