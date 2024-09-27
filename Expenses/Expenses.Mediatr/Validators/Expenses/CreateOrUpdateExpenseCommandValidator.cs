using Expenses.Mediatr.Mediatr.Expenses.Commands;
using FluentValidation;

namespace Expenses.Mediatr.Validators.Expenses;

public class CreateOrUpdateExpenseCommandValidator : AbstractValidator<CreateOrUpdateExpenseCommand>
{
    public CreateOrUpdateExpenseCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must be at most 100 characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage("Description must be at most 300 characters long.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");
    }
}