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

public class RemoveExpenseCommandHandler: MediatrAuthBase, IRequestHandler<RemoveExpenseCommand>
{
    private readonly IBalanceRepository balanceRepository;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository;
    private readonly IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;

    public RemoveExpenseCommandHandler(
        IAuthRepository authRepository,
        IBalanceRepository balanceRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
    ) : base(authRepository)
    {
        this.balanceRepository = balanceRepository;
        this.entityValidator = entityValidator;
        this.expenseRepository = expenseRepository;
        this.userProjectRepository = userProjectRepository;
    }
    
    public async Task Handle(RemoveExpenseCommand command, CancellationToken cancellationToken)
    {
        Expense expense = await this.expenseRepository.GetByIdAsync(command.Id, cancellationToken);
        this.entityValidator.ValidateExist(expense, command.Id);
        
        Guid userId = await this.GetCurrentUserIdAsync();
        
        UserProject userProject =
            await this.userProjectRepository.GetAsync(
                up => up.Id == expense.UserProjectId, 
                cancellationToken,
                up => up.Include(a => a.AllowedUsers).Include(b => b.Balances));
        this.entityValidator.ValidateExist(userProject, expense.UserProjectId);
        
        if (userProject.CreatedUserId != userId ||
            userProject.AllowedUsers.All(au => au.UserId != userId))
        {
            throw new BusinessException(ErrorMessages.Forbidden, 403);
        }

        await this.balanceRepository.RemoveExpenseAsync(expense, cancellationToken);
    }
}