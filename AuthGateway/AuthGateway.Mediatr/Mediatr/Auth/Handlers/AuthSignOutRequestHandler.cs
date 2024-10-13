using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class AuthSignOutRequestHandler: MediatrAuthBase, IRequestHandler<AuthSignOutRequest, bool>
{
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IGenericRepository<Guid, User, AuthGatewayDataContext> userRepository;
    private readonly ITokenRepository tokenService;
    
    public AuthSignOutRequestHandler(
        IAuthRepository authRepository,
        IEntityValidator<AuthGatewayDataContext> entityValidator,
        IGenericRepository<Guid, User, AuthGatewayDataContext> userRepository,
        ITokenRepository tokenService): base(authRepository)
    {
        this.entityValidator = entityValidator;
        this.userRepository = userRepository;
        this.tokenService = tokenService;
    }
    
    
    public async Task<bool> Handle(AuthSignOutRequest request, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetCurrentUserIdAsync();
        User user = await this.userRepository.GetByIdAsync(userId, cancellationToken);
        this.entityValidator.IsEntityExist(user);
        this.entityValidator.IsEntityActive(user);

        await this.tokenService.RemoveUserTokenAsync(user.Id);

        return true;
    }
}