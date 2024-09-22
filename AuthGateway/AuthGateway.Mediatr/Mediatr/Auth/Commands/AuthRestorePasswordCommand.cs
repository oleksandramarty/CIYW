using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Commands;

public class AuthRestorePasswordCommand: IRequest
{
    public string Url { get; set; }
    public string Password { get; set; }
    public string PasswordAgain { get; set; }
}