using Expenses.Mediatr.Mediatr.Expenses.Commands;
using FluentValidation;

namespace Expenses.Mediatr.Validators.Expenses;

public class UpdateFavoriteExpenseCommandValidator : AbstractValidator<UpdateFavoriteExpenseCommand>
{
    public UpdateFavoriteExpenseCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(50).WithMessage("Title must be at most 50 characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(100).WithMessage("Description must be at most 100 characters long.");
    }
}