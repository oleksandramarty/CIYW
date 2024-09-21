using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using FluentValidation;

namespace AuthGateway.Mediatr.Validators.Auth;

public class AuthSignUpCommandValidator : AbstractValidator<AuthSignUpCommand>
{
    public AuthSignUpCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Login is required.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,}$")
            .WithMessage("Password must be at least 10 characters long and contain at least one digit, one uppercase letter, one lowercase letter, and one special symbol.");
        
        RuleFor(x => x.PasswordAgain)
            .NotEmpty().WithMessage("Password is required.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,}$")
            .WithMessage("Password must be at least 10 characters long and contain at least one digit, one uppercase letter, one lowercase letter, and one special symbol.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.")
            .Matches(@"^[^\s@]+@[^\s@]+\.[^\s@]+$").WithMessage("Email must be in a valid format.");

        RuleFor(x => x)
            .Must(x => x.Password == x.PasswordAgain)
            .WithMessage("Password and PasswordAgain must be the same.");
    }
}