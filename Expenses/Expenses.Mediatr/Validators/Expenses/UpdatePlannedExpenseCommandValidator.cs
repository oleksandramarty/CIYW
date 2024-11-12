using Expenses.Mediatr.Mediatr.Expenses.Commands;
using FluentValidation;

namespace Expenses.Mediatr.Validators.Expenses;

public class UpdatePlannedExpenseCommandValidator: AbstractValidator<UpdatePlannedExpenseCommand>
{
    public UpdatePlannedExpenseCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(50).WithMessage("Title must be at most 50 characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(100).WithMessage("Description must be at most 100 characters long.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");
        
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");
        
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).When(x => x.EndDate.HasValue).WithMessage("End date must be greater than start date if it is provided.");
    }
}