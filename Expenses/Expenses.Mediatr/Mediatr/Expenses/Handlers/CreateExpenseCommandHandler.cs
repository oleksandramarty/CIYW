using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Extensions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using Expenses.Business;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Validators.Expenses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class CreateExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<CreateExpenseCommand>
{
    private readonly IMapper mapper;
    private readonly IBalanceRepository balanceRepository;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> expenseRepository;
    private readonly IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository;

    public CreateExpenseCommandHandler(
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

    public async Task Handle(CreateExpenseCommand command, CancellationToken cancellationToken)
    {        
        this.entityValidator.ValidateVoidRequest<CreateExpenseCommand>(command, () => new CreateExpenseCommandValidator());

        await this.CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);

        DateTime currentMonth = DateTimeExtension.GetStartOfCurrentMonth();
        
        if (await this.expenseRepository.Queryable(fe => 
                    fe.UserProjectId == command.UserProjectId &&
                    fe.CreatedAt >= currentMonth
                    )
                .CountAsync(cancellationToken) >= 50)
        {
            throw new BusinessException(ErrorMessages.UserProjectLimitExceeded, 409);
        }
    
        ExpenseEntity toAdd = this.mapper.Map<ExpenseEntity>(command);
        await this.balanceRepository.AddExpenseAsync(toAdd, cancellationToken);
        return;
    }
}