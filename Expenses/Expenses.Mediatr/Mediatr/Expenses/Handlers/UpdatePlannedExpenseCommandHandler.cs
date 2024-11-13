using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Validators.Expenses;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class UpdatePlannedExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<UpdatePlannedExpenseCommand>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, PlannedExpenseEntity, ExpensesDataContext> plannedExpenseRepository;

    public UpdatePlannedExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, PlannedExpenseEntity, ExpensesDataContext> plannedExpenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository
        ) : base(currentUserRepository, entityValidator, userProjectRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.plannedExpenseRepository = plannedExpenseRepository;
    }

    public async Task Handle(UpdatePlannedExpenseCommand command, CancellationToken cancellationToken)
    {        
        this.entityValidator.ValidateVoidRequest<UpdatePlannedExpenseCommand>(command, () => new UpdatePlannedExpenseCommandValidator());
        
        PlannedExpenseEntity currentPlannedExpenseEntity = await this.plannedExpenseRepository.GetAsync(
            e => e.Id == command.Id, cancellationToken);
        this.entityValidator.IsEntityExist(currentPlannedExpenseEntity);
        
        await this.CheckUserProjectByIdAsync(currentPlannedExpenseEntity.UserProjectId, cancellationToken);
        
        await this.plannedExpenseRepository.UpdateAsync(
            this.mapper.Map<UpdatePlannedExpenseCommand, PlannedExpenseEntity>(command, currentPlannedExpenseEntity), cancellationToken);
    }
}