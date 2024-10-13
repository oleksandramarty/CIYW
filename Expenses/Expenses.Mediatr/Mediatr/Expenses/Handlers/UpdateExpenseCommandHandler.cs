using AuthGateway.Mediatr.Validators.Auth;
using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using Expenses.Business;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Validators.Expenses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class UpdateExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<UpdateExpenseCommand>
{
    private readonly IMapper mapper;
    private readonly IBalanceRepository balanceRepository;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository;
    private readonly IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;

    public UpdateExpenseCommandHandler(
        IAuthRepository authRepository,
        IMapper mapper,
        IBalanceRepository balanceRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
        ) : base(authRepository, entityValidator, userProjectRepository)
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

        Expense currentExpense = await this.expenseRepository.GetAsync(
            e => e.Id == command.Id, cancellationToken);
        this.entityValidator.IsEntityExist(currentExpense);
        
        await this.CheckUserProjectByIdAsync(currentExpense.UserProjectId, cancellationToken);
        
        await this.balanceRepository.UpdateExpenseAsync(currentExpense,
            this.mapper.Map<Expense>(command, opts => opts.Items["IsUpdate"] = true), cancellationToken);
    }
}