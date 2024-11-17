using System.Net;
using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Responses.AuthGateway.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class CurrentUserRequestHandler: MediatrAuthBase, IRequestHandler<CurrentUserRequest, UserResponse>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository;
    private readonly IGenericRepository<Guid, UserRoleEntity, AuthGatewayDataContext> userRoleRepository;

    public CurrentUserRequestHandler(
        ICurrentUserRepository currentUserRepository,
        IMediator mediator,
        IMapper mapper, 
        IEntityValidator<AuthGatewayDataContext> entityValidator, 
        IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository,
        IGenericRepository<Guid, UserRoleEntity, AuthGatewayDataContext> userRoleRepository): base(currentUserRepository)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.userRepository = userRepository;
        this.userRoleRepository = userRoleRepository;
    }
    
    public async Task<UserResponse> Handle(CurrentUserRequest request, CancellationToken cancellationToken)
    {
        Guid userId = await this.CurrentUserIdAsync();
        
        UserEntity? user = await this.userRepository.ByIdAsync(userId, cancellationToken, 
            user => 
                user
                    .Include(u => u.Roles)
                    .ThenInclude(ur => ur.Role)
                    .Include(u => u.UserSetting));
        if (user == null)
        {
            throw new EntityNotFoundException();
        }
        if (user.IsActive == false)
        {
            throw new BusinessException(ErrorMessages.EntityBlocked, (int)HttpStatusCode.Conflict);
        }
        
        UserResponse response = this.mapper.Map<UserEntity, UserResponse>(user);

        return response;
    }
}