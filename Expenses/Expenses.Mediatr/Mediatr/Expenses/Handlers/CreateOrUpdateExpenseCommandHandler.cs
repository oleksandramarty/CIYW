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

public class CreateOrUpdateExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<CreateOrUpdateExpenseCommand>
{
    private readonly IMapper mapper;
    private readonly IBalanceRepository balanceRepository;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository;
    private readonly IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;

    public CreateOrUpdateExpenseCommandHandler(
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

    public async Task Handle(CreateOrUpdateExpenseCommand command, CancellationToken cancellationToken)
    {        
        this.entityValidator.ValidateVoidRequest<CreateOrUpdateExpenseCommand>(command, () => new CreateOrUpdateExpenseCommandValidator());

        await this.CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);

        if (!command.Id.HasValue)
        {
            Expense toAdd = this.mapper.Map<Expense>(command, opts => opts.Items["IsUpdate"] = false);
            toAdd.Version = Guid.NewGuid().ToString("N").ToUpper();
            await this.balanceRepository.AddExpenseAsync(toAdd, cancellationToken);
            return;
        }
        
        Expense currentExpense = await this.expenseRepository.GetAsync(
            e => e.Id == command.Id.Value, cancellationToken);
        this.entityValidator.ValidateExist(currentExpense, command.Id.Value);
        
        if (currentExpense.UserProjectId != command.UserProjectId)
        {
            throw new ForbiddenException();
        }
        
        currentExpense.Version = Guid.NewGuid().ToString("N").ToUpper();
        await this.balanceRepository.UpdateExpenseAsync(currentExpense,
            this.mapper.Map<Expense>(command, opts => opts.Items["IsUpdate"] = true), cancellationToken);
    }
}