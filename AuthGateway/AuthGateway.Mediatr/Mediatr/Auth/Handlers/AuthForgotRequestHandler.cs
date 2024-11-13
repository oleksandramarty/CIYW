using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Extensions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class AuthForgotRequestHandler : MediatrAuthBase, IRequestHandler<AuthForgotRequest>
{
    private readonly ICurrentUserRepository currentUserRepository;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository;

    public AuthForgotRequestHandler(
        ICurrentUserRepository currentUserRepository,
        IEntityValidator<AuthGatewayDataContext> entityValidator,
        IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository
    ) : base(currentUserRepository)
    {
        this.entityValidator = entityValidator;
        this.userRepository = userRepository;
    }

    public async Task Handle(AuthForgotRequest request, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetCurrentUserIdAsync();
        UserEntity userEntity = await this.userRepository.GetByIdAsync(userId, cancellationToken);
        this.entityValidator.IsEntityExist(userEntity);
        this.entityValidator.IsEntityActive(userEntity);

        if (userEntity.LastForgotPasswordRequest.HasValue &&
            userEntity.LastForgotPasswordRequest.Value.AddMinutes(30) > DateTime.UtcNow)
        {
            throw new BusinessException(ErrorMessages.ForgotPasswordRequestTooSoon, StatusCodes.Status409Conflict);
        }

        userEntity.LastForgotPasswordRequest = DateTime.UtcNow;
        await this.userRepository.UpdateAsync(userEntity, cancellationToken);

        // TODO Send email
        string restoreLink =
            $"{StringExtension.InterleaveStrings(userId.ToString("N"), Guid.NewGuid().ToString("N"))}" +
            $"&honkler={StringExtension.InterleaveStrings(
                (new DateTimeOffset(userEntity.LastForgotPasswordRequest.Value).ToUnixTimeSeconds()).ToString(),
                (new DateTimeOffset(userEntity.CreatedAt).ToUnixTimeSeconds()).ToString())}";
    }
}