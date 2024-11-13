using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Responses.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class AuthSignInRequestHandler: IRequestHandler<AuthSignInRequest, JwtTokenResponse>
{
    private readonly IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository;
    private readonly ITokenRepository tokenRepository;
    private readonly IJwtTokenFactory jwtTokenFactory;

    public AuthSignInRequestHandler(
        IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository,
        ITokenRepository tokenRepository,
        IJwtTokenFactory jwtTokenFactory)
    {
        this.userRepository = userRepository;
        this.tokenRepository = tokenRepository;
        this.jwtTokenFactory = jwtTokenFactory;
    }

    public async Task<JwtTokenResponse> Handle(AuthSignInRequest request, CancellationToken cancellationToken)
    {
        UserEntity userEntity = await this.userRepository.GetAsync(u => 
                u.Email == request.Login || 
                u.Login == request.Login, 
            cancellationToken,
            user => user.Include(u => u.Roles).ThenInclude(ur => ur.Role));
        if (userEntity == null)
        {
            throw new EntityNotFoundException();
        }

        var hashedPassword = this.jwtTokenFactory.HashPassword(request.Password, userEntity.Salt);
        if (hashedPassword != userEntity.PasswordHash)
        {
            throw new AuthException(ErrorMessages.WrongAuth, StatusCodes.Status403Forbidden);
        }

        var token = this.jwtTokenFactory.GenerateJwtToken(
            userEntity.Id,
            userEntity.Login,
            userEntity.Email,
            string.Join(",", userEntity.Roles.Select(r => r.Role.Title)),
            request.RememberMe);

        await this.tokenRepository.AddTokenAsync(token, request.RememberMe ? TimeSpan.FromDays(30) : TimeSpan.FromDays(1));

        return new JwtTokenResponse
        {
            Token = token
        };
    }
}