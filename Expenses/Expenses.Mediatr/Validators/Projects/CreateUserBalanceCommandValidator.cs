using Expenses.Mediatr.Mediatr.Projects.Commands;
using FluentValidation;

namespace Expenses.Mediatr.Validators.Projects;

public class CreateUserBalanceCommandValidator : AbstractValidator<CreateUserBalanceCommand>
{
    public CreateUserBalanceCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must be at most 100 characters long.");
    }
}