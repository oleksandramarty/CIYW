using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AuthGateway.Mediatr.Validators.Auth;
using AutoMapper;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class AuthSignUpCommandHandler: IRequestHandler<AuthSignUpCommand>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IJwtTokenFactory jwtTokenFactory;
    private readonly IGenericRepository<Guid, User, AuthGatewayDataContext> userRepository;
    private readonly IGenericRepository<Guid, UserRole, AuthGatewayDataContext> userRoleRepository;

    public AuthSignUpCommandHandler(
        IMediator mediator,
        IMapper mapper, 
        IEntityValidator<AuthGatewayDataContext> entityValidator,
        IJwtTokenFactory jwtTokenFactory,
        IGenericRepository<Guid, User, AuthGatewayDataContext> userRepository,
        IGenericRepository<Guid, UserRole, AuthGatewayDataContext> userRoleRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.jwtTokenFactory = jwtTokenFactory;
        this.userRepository = userRepository;
        this.userRoleRepository = userRoleRepository;
    }

    public async Task Handle(AuthSignUpCommand command, CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateVoidRequest<AuthSignUpCommand>(command, () => new AuthSignUpCommandValidator());
        
        await this.entityValidator.ValidateExistParamAsync<User>(
            u => u.Email == command.Email, 
            ErrorMessages.EntityWithEmailAlreadyExists, 
            cancellationToken);

        User user = this.mapper.Map<AuthSignUpCommand, User>(command);
        
        user.Salt = this.jwtTokenFactory.GenerateSalt();
        user.PasswordHash = this.jwtTokenFactory.HashPassword(command.Password, user.Salt);
        
        await this.userRepository.AddAsync(user, cancellationToken);
        await this.userRoleRepository.AddAsync(new UserRole
        {
            RoleId = (int)command.Role,
            UserId = user.Id
        }, cancellationToken);
    }
}