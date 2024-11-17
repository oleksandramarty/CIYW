using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Responses.Base;
using Expenses.Business;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class RemoveExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<RemoveExpenseCommand, BaseBoolResponse>
{
    private readonly IBalanceRepository balanceRepository;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> expenseRepository;

    public RemoveExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IBalanceRepository balanceRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> expenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository
    ) : base(currentUserRepository, entityValidator, userProjectRepository)
    {
        this.balanceRepository = balanceRepository;
        this.entityValidator = entityValidator;
        this.expenseRepository = expenseRepository;
    }
    
    public async Task<BaseBoolResponse> Handle(RemoveExpenseCommand command, CancellationToken cancellationToken)
    {
        ExpenseEntity? expense = await this.expenseRepository.ByIdAsync(command.Id, cancellationToken);
        if (expense == null)
        {
            throw new EntityNotFoundException();
        }

        await this.CheckUserProjectByIdAsync(expense.UserProjectId, cancellationToken);

        await this.balanceRepository.RemoveExpenseAsync(expense, cancellationToken);

        return new BaseBoolResponse();
    }
}