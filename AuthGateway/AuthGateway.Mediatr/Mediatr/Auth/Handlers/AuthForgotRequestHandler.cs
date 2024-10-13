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
    private readonly IAuthRepository authRepository;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IGenericRepository<Guid, User, AuthGatewayDataContext> userRepository;

    public AuthForgotRequestHandler(
        IAuthRepository authRepository,
        IEntityValidator<AuthGatewayDataContext> entityValidator,
        IGenericRepository<Guid, User, AuthGatewayDataContext> userRepository
    ) : base(authRepository)
    {
        this.entityValidator = entityValidator;
        this.userRepository = userRepository;
    }

    public async Task Handle(AuthForgotRequest request, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetCurrentUserIdAsync();
        User user = await this.userRepository.GetByIdAsync(userId, cancellationToken);
        this.entityValidator.IsEntityExist(user);
        this.entityValidator.IsEntityActive(user);

        if (user.LastForgotPasswordRequest.HasValue &&
            user.LastForgotPasswordRequest.Value.AddMinutes(30) > DateTime.UtcNow)
        {
            throw new BusinessException(ErrorMessages.ForgotPasswordRequestTooSoon, StatusCodes.Status409Conflict);
        }

        user.LastForgotPasswordRequest = DateTime.UtcNow;
        await this.userRepository.UpdateAsync(user, cancellationToken);

        // TODO Send email
        string restoreLink =
            $"{StringExtension.InterleaveStrings(userId.ToString("N"), Guid.NewGuid().ToString("N"))}" +
            $"&honkler={StringExtension.InterleaveStrings(
                (new DateTimeOffset(user.LastForgotPasswordRequest.Value).ToUnixTimeSeconds()).ToString(),
                (new DateTimeOffset(user.Created).ToUnixTimeSeconds()).ToString())}";
    }
}