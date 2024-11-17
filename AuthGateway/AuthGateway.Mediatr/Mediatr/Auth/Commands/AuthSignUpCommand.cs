using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Enums;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Commands;

public class AuthSignUpCommand: IRequest
{
    [Required] [MaxLength(50)] public required string Login { get; set; }
    [Required] [MaxLength(50)] public required string Email { get; set; }
    [Required] [MaxLength(50)] public required string Password { get; set; }
    [Required] [MaxLength(50)] public required string PasswordAgain { get; set; }
    public UserRoleEnum Role { get; set; }
}