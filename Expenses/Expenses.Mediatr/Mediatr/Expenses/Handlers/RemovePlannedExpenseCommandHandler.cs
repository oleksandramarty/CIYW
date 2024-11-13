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
        PlannedExpenseEntity plannedExpenseEntity = await this.plannedExpenseRepository.GetByIdAsync(command.Id, cancellationToken);
        this.entityValidator.IsEntityExist(plannedExpenseEntity);

        await this.CheckUserProjectByIdAsync(plannedExpenseEntity.UserProjectId, cancellationToken);

        await this.plannedExpenseRepository.DeleteByIdAsync(command.Id, cancellationToken);

        return new BaseBoolResponse();
    }
}