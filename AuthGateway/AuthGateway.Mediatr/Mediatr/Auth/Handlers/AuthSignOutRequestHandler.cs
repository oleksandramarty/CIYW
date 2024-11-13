using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class AuthSignOutRequestHandler: MediatrAuthBase, IRequestHandler<AuthSignOutRequest, BaseBoolResponse>
{
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository;
    private readonly ITokenRepository tokenService;
    
    public AuthSignOutRequestHandler(
        ICurrentUserRepository currentUserRepository,
        IEntityValidator<AuthGatewayDataContext> entityValidator,
        IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository,
        ITokenRepository tokenService): base(currentUserRepository)
    {
        this.entityValidator = entityValidator;
        this.userRepository = userRepository;
        this.tokenService = tokenService;
    }
    
    
    public async Task<BaseBoolResponse> Handle(AuthSignOutRequest request, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetCurrentUserIdAsync();
        UserEntity userEntity = await this.userRepository.GetByIdAsync(userId, cancellationToken);
        this.entityValidator.IsEntityExist(userEntity);
        this.entityValidator.IsEntityActive(userEntity);

        await this.tokenService.RemoveUserTokenAsync(userEntity.Id);

        return new BaseBoolResponse();
    }
}