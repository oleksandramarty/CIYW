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
    private readonly IBalanceRepository _balanceRepository;
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> _readGenericExpenseRepository;

    public RemoveExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IBalanceRepository balanceRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> readGenericExpenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository
    ) : base(currentUserRepository, entityValidator, readGenericUserProjectRepository)
    {
        _balanceRepository = balanceRepository;
        _entityValidator = entityValidator;
        _readGenericExpenseRepository = readGenericExpenseRepository;
    }
    
    public async Task<BaseBoolResponse> Handle(RemoveExpenseCommand command, CancellationToken cancellationToken)
    {
        ExpenseEntity? expense = await _readGenericExpenseRepository.ByIdAsync(command.Id, cancellationToken);
        if (expense == null)
        {
            throw new EntityNotFoundException();
        }

        await CheckUserProjectByIdAsync(expense.UserProjectId, cancellationToken);

        await _balanceRepository.RemoveExpenseAsync(expense, cancellationToken);

        return new BaseBoolResponse();
    }
}