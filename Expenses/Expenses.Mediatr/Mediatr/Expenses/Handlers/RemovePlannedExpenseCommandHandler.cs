using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class RemovePlannedExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<RemovePlannedExpenseCommand, BaseBoolResponse>
{
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, PlannedExpenseEntity, ExpensesDataContext> plannedExpenseRepository;

    public RemovePlannedExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, PlannedExpenseEntity, ExpensesDataContext> plannedExpenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository
    ) : base(currentUserRepository, entityValidator, userProjectRepository)
    {
        this.entityValidator = entityValidator;
        this.plannedExpenseRepository = plannedExpenseRepository;
    }
    
    public async Task<BaseBoolResponse> Handle(RemovePlannedExpenseCommand command, CancellationToken cancellationToken)
    {
        PlannedExpenseEntity? plannedExpense = await this.plannedExpenseRepository.ByIdAsync(command.Id, cancellationToken);
        if (plannedExpense == null)
        {
            throw new EntityNotFoundException();
        }

        await this.CheckUserProjectByIdAsync(plannedExpense.UserProjectId, cancellationToken);

        await this.plannedExpenseRepository.DeleteByIdAsync(command.Id, cancellationToken);

        return new BaseBoolResponse();
    }
}