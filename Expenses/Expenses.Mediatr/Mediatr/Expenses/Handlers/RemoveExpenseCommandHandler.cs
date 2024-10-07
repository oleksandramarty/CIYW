using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using Expenses.Business;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class RemoveExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<RemoveExpenseCommand, bool>
{
    private readonly IBalanceRepository balanceRepository;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository;

    public RemoveExpenseCommandHandler(
        IAuthRepository authRepository,
        IBalanceRepository balanceRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
    ) : base(authRepository, entityValidator, userProjectRepository)
    {
        this.balanceRepository = balanceRepository;
        this.entityValidator = entityValidator;
        this.expenseRepository = expenseRepository;
    }
    
    public async Task<bool> Handle(RemoveExpenseCommand command, CancellationToken cancellationToken)
    {
        Expense expense = await this.expenseRepository.GetByIdAsync(command.Id, cancellationToken);
        this.entityValidator.ValidateExist(expense, command.Id);

        await this.CheckUserProjectByIdAsync(expense.UserProjectId, cancellationToken);

        await this.balanceRepository.RemoveExpenseAsync(expense, cancellationToken);

        return true;
    }
}