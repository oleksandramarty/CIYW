using System.Net;
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
        Guid userId = await this.CurrentUserIdAsync();
        UserEntity? user = await this.userRepository.ByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException();
        }
        if (user.IsActive == false)
        {
            throw new BusinessException(ErrorMessages.EntityBlocked, (int)HttpStatusCode.Conflict);
        }

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
                (new DateTimeOffset(user.CreatedAt).ToUnixTimeSeconds()).ToString())}";
    }
}