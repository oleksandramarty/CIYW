using Expenses.Mediatr.Mediatr.Projects.Commands;
using FluentValidation;

namespace Expenses.Mediatr.Validators.Projects;

public class CreateUserProjectCommandValidator : AbstractValidator<CreateUserProjectCommand>
{
    public CreateUserProjectCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must be at most 100 characters long.");
    }
}