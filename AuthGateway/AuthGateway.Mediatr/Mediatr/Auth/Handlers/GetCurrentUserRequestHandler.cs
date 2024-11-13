using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using AutoMapper;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.AuthGateway.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class GetCurrentUserRequestHandler: MediatrAuthBase, IRequestHandler<GetCurrentUserRequest, UserResponse>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository;
    private readonly IGenericRepository<Guid, UserRoleEntity, AuthGatewayDataContext> userRoleRepository;

    public GetCurrentUserRequestHandler(
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
    
    public async Task<UserResponse> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetCurrentUserIdAsync();
        
        UserEntity userEntity = await this.userRepository.GetByIdAsync(userId, cancellationToken, 
            user => 
                user
                    .Include(u => u.Roles)
                    .ThenInclude(ur => ur.Role)
                    .Include(u => u.UserSetting));
        this.entityValidator.IsEntityExist(userEntity);
        this.entityValidator.IsEntityActive(userEntity);
        
        UserResponse response = this.mapper.Map<UserEntity, UserResponse>(userEntity);

        return response;
    }
}