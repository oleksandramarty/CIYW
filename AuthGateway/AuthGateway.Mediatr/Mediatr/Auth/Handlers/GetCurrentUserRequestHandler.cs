using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using AutoMapper;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Responses.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class GetCurrentUserRequestHandler: MediatrAuthBase, IRequestHandler<GetCurrentUserRequest, UserResponse>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IGenericRepository<Guid, User, AuthGatewayDataContext> userRepository;
    private readonly IGenericRepository<Guid, UserRole, AuthGatewayDataContext> userRoleRepository;

    public GetCurrentUserRequestHandler(
        IAuthRepository authRepository,
        IMediator mediator,
        IMapper mapper, 
        IEntityValidator<AuthGatewayDataContext> entityValidator, 
        IGenericRepository<Guid, User, AuthGatewayDataContext> userRepository,
        IGenericRepository<Guid, UserRole, AuthGatewayDataContext> userRoleRepository): base(authRepository)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.userRepository = userRepository;
        this.userRoleRepository = userRoleRepository;
    }
    
    public async Task<UserResponse> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetUserIdAsync();
        
        User user = await this.userRepository.GetByIdAsync(userId, cancellationToken, 
            user => user.Include(u => u.Roles).ThenInclude(ur => ur.Role));
        this.entityValidator.ValidateExist<User, Guid?>(user, userId);
        
        UserResponse response = this.mapper.Map<User, UserResponse>(user);

        return response;
    }
}