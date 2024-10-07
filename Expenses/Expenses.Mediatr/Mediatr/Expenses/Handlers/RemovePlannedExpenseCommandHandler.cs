using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class RemovePlannedExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<RemovePlannedExpenseCommand, bool>
{
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, PlannedExpense, ExpensesDataContext> plannedExpenseRepository;

    public RemovePlannedExpenseCommandHandler(
        IAuthRepository authRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, PlannedExpense, ExpensesDataContext> plannedExpenseRepository,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
    ) : base(authRepository, entityValidator, userProjectRepository)
    {
        this.entityValidator = entityValidator;
        this.plannedExpenseRepository = plannedExpenseRepository;
    }
    
    public async Task<bool> Handle(RemovePlannedExpenseCommand command, CancellationToken cancellationToken)
    {
        PlannedExpense plannedExpense = await this.plannedExpenseRepository.GetByIdAsync(command.Id, cancellationToken);
        this.entityValidator.ValidateExist(plannedExpense, command.Id);

        await this.CheckUserProjectByIdAsync(plannedExpense.UserProjectId, cancellationToken);

        await this.plannedExpenseRepository.RemoveByIdAsync(command.Id, cancellationToken);

        return true;
    }
}