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

public class UserProjectByIdRequestHandler: MediatrExpensesBase, IRequestHandler<UserProjectByIdRequest, UserProjectResponse>
{
    private readonly IMapper _mapper;
    
    public UserProjectByIdRequestHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository
        ): base(currentUserRepository, entityValidator, readGenericUserProjectRepository)
    {
        _mapper = mapper;
    }
    
    public async Task<UserProjectResponse> Handle(UserProjectByIdRequest command, CancellationToken cancellationToken)
    {
        Guid userId = await CurrentUserIdAsync();
        UserProjectEntity userProjectEntity = await UserProjectByIdAsync(command.Id, cancellationToken);
        
        if (userProjectEntity.CreatedUserId != userId && userProjectEntity.AllowedUsers.All(au => au.UserId != userId))
        {
            throw new ForbiddenException();
        }
        
        return _mapper.Map<UserProjectResponse>(userProjectEntity);
    }
}