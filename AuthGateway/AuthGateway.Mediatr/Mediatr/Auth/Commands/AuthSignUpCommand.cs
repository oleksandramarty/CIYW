using CommonModule.Shared.Common;
using CommonModule.Shared.Enums;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Commands;

public class AuthSignUpCommand: BaseIdEntity<Guid?>, IRequest
{
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordAgain { get; set; }
    public UserRoleEnum Role { get; set; }
}