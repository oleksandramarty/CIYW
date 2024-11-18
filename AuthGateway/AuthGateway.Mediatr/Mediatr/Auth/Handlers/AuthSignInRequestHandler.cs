using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using AuthGateway.Mediatr.Validators.Auth;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Responses.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class AuthSignInRequestHandler : IRequestHandler<AuthSignInRequest, JwtTokenResponse>
{
    private readonly IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly ITokenRepository tokenRepository;
    private readonly IJwtTokenFactory jwtTokenFactory;

    public AuthSignInRequestHandler(
        IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository,
        IEntityValidator<AuthGatewayDataContext> entityValidator,
        ITokenRepository tokenRepository,
        IJwtTokenFactory jwtTokenFactory)
    {
        this.userRepository = userRepository;
        this.entityValidator = entityValidator;
        this.tokenRepository = tokenRepository;
        this.jwtTokenFactory = jwtTokenFactory;
    }

    public async Task<JwtTokenResponse> Handle(AuthSignInRequest request, CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateRequest<AuthSignInRequest, JwtTokenResponse>(request,
            () => new AuthSignInRequestValidator());

        UserEntity user = await this.userRepository.Async(u =>
                u.Email == request.Login ||
                u.Login == request.Login,
            cancellationToken,
            user => user.Include(u => u.Roles).ThenInclude(ur => ur.Role));
        if (user == null)
        {
            throw new EntityNotFoundException();
        }

        if (!user.IsActive)
        {
            throw new BusinessException(ErrorMessages.UserBlocked, StatusCodes.Status403Forbidden);
        }

        var hashedPassword = this.jwtTokenFactory.HashPassword(request.Password, user.Salt);
        if (hashedPassword != user.PasswordHash)
        {
            throw new AuthException(ErrorMessages.WrongAuth, StatusCodes.Status403Forbidden);
        }

        var token = this.jwtTokenFactory.GenerateJwtToken(
            user.Id,
            user.Login,
            user.Email,
            string.Join(",", user.Roles.Select(r => r.Role?.Title)),
            request.RememberMe);

        await this.tokenRepository.AddTokenAsync(token,
            request.RememberMe ? TimeSpan.FromDays(30) : TimeSpan.FromDays(1));

        return new JwtTokenResponse
        {
            Token = token
        };
    }
}