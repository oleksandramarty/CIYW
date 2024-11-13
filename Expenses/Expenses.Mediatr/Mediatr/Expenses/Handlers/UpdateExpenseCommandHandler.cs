using AutoMapper;
using CommonModule.Interfaces;
using Expenses.Business;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Validators.Expenses;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class UpdateExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<UpdateExpenseCommand>
{
    private readonly IMapper mapper;
    private readonly IBalanceRepository balanceRepository;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> expenseRepository;
    private readonly IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository;

    public UpdateExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IBalanceRepository balanceRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> expenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository
        ) : base(currentUserRepository, entityValidator, userProjectRepository)
    {
        this.mapper = mapper;
        this.balanceRepository = balanceRepository;
        this.entityValidator = entityValidator;
        this.expenseRepository = expenseRepository;
        this.userProjectRepository = userProjectRepository;
    }

    public async Task Handle(UpdateExpenseCommand command, CancellationToken cancellationToken)
    {        
        this.entityValidator.ValidateVoidRequest<UpdateExpenseCommand>(command, () => new UpdateExpenseCommandValidator());

        ExpenseEntity currentExpenseEntity = await this.expenseRepository.GetAsync(
            e => e.Id == command.Id, cancellationToken);
        this.entityValidator.IsEntityExist(currentExpenseEntity);
        
        await this.CheckUserProjectByIdAsync(currentExpenseEntity.UserProjectId, cancellationToken);
        
        await this.balanceRepository.UpdateExpenseAsync(currentExpenseEntity,
            this.mapper.Map<ExpenseEntity>(command), cancellationToken);
    }
}