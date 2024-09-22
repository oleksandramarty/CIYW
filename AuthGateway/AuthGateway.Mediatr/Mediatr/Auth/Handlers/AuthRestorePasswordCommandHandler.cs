using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AuthGateway.Mediatr.Validators.Auth;
using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class AuthRestorePasswordCommandHandler: IRequestHandler<AuthRestorePasswordCommand>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IJwtTokenFactory jwtTokenFactory;
    private readonly IGenericRepository<Guid, User, AuthGatewayDataContext> userRepository;
    private readonly IGenericRepository<Guid, UserRole, AuthGatewayDataContext> userRoleRepository;

    public AuthRestorePasswordCommandHandler(
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

    public async Task Handle(AuthRestorePasswordCommand command, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(command.Url))
        {
            throw new AuthException(ErrorMessages.RestorePasswordProcessingIssue, 409);
        }
        
        this.entityValidator.ValidateVoidRequest<AuthRestorePasswordCommand>(command, () => new AuthRestorePasswordCommandValidator());
        // TODO restore and check 3 questions
    }
}