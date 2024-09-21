using CommonModule.Shared.Responses.Auth;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Requests;

public class AuthLoginRequest: IRequest<JwtTokenResponse>
{
    public string Login { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}