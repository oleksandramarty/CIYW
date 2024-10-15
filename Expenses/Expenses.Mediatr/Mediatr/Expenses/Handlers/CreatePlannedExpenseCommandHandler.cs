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

public class CreatePlannedExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<CreatePlannedExpenseCommand>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, PlannedExpense, ExpensesDataContext> plannedExpenseRepository;

    public CreatePlannedExpenseCommandHandler(
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
    }

    public async Task Handle(CreatePlannedExpenseCommand command, CancellationToken cancellationToken)
    {        
        this.entityValidator.ValidateVoidRequest<CreatePlannedExpenseCommand>(command, () => new CreatePlannedExpenseCommandValidator());

        await this.CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);

        PlannedExpense toAdd = this.mapper.Map<PlannedExpense>(command, opts => opts.Items["IsUpdate"] = false);
            
        await this.plannedExpenseRepository.AddAsync(toAdd, cancellationToken);
        return;
    }
}