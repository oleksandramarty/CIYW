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

public class CreateOrUpdatePlannedExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<CreateOrUpdatePlannedExpenseCommand>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, PlannedExpense, ExpensesDataContext> plannedExpenseRepository;
    private readonly IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;

    public CreateOrUpdatePlannedExpenseCommandHandler(
        IAuthRepository authRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, PlannedExpense, ExpensesDataContext> plannedExpenseRepository,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
        ) : base(authRepository, entityValidator, userProjectRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.plannedExpenseRepository = plannedExpenseRepository;
        this.userProjectRepository = userProjectRepository;
    }

    public async Task Handle(CreateOrUpdatePlannedExpenseCommand command, CancellationToken cancellationToken)
    {        
        this.entityValidator.ValidateVoidRequest<CreateOrUpdatePlannedExpenseCommand>(command, () => new CreateOrUpdatePlannedExpenseCommandValidator());

        await this.CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);

        if (!command.Id.HasValue)
        {
            PlannedExpense toAdd = this.mapper.Map<PlannedExpense>(command, opts => opts.Items["IsUpdate"] = false);
            toAdd.Version = Guid.NewGuid().ToString("N").ToUpper();
            
            await this.plannedExpenseRepository.AddAsync(toAdd, cancellationToken);
            return;
        }
        
        PlannedExpense currentPlannedExpense = await this.plannedExpenseRepository.GetAsync(
            e => e.Id == command.Id.Value, cancellationToken);
        this.entityValidator.ValidateExist(currentPlannedExpense, command.Id.Value);
        
        if (currentPlannedExpense.UserProjectId != command.UserProjectId)
        {
            throw new ForbiddenException();
        }
        
        currentPlannedExpense.Version = Guid.NewGuid().ToString("N").ToUpper();

        await this.plannedExpenseRepository.UpdateAsync(
            this.mapper.Map<PlannedExpense>(command, opts => opts.Items["IsUpdate"] = true), cancellationToken);
    }
}