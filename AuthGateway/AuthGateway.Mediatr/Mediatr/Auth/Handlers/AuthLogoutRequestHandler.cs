using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class AuthLogoutRequestHandler: MediatrAuthBase, IRequestHandler<AuthLogoutRequest>
{
    private readonly IEntityValidator<AuthDataContext> entityValidator;
    private readonly IGenericRepository<Guid, User, AuthDataContext> userRepository;
    private readonly ITokenRepository tokenService;
    
    public AuthLogoutRequestHandler(
        IAuthRepository authRepository,
        IEntityValidator<AuthDataContext> entityValidator,
        IGenericRepository<Guid, User, AuthDataContext> userRepository,
        ITokenRepository tokenService): base(authRepository)
    {
        this.entityValidator = entityValidator;
        this.userRepository = userRepository;
        this.tokenService = tokenService;
    }
    
    
    public async Task Handle(AuthLogoutRequest request, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetUserIdAsync();
        User user = await this.userRepository.GetByIdAsync(userId, cancellationToken);
        this.entityValidator.ValidateExist<User, Guid>(user, userId);

        await this.tokenService.RemoveUserTokenAsync(user.Id);
    }
}