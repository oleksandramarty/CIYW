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

public class CreateOrUpdateExpenseCommandHandler: MediatrAuthBase, IRequestHandler<CreateOrUpdateExpenseCommand>
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
        ) : base(authRepository)
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

        Guid userId = await this.GetCurrentUserIdAsync();

        UserProject userProject =
            await this.userProjectRepository.GetAsync(
                up => up.Id == command.UserProjectId, 
                cancellationToken,
                up => up.Include(a => a.AllowedUsers).Include(b => b.Balances));
        this.entityValidator.ValidateExist(userProject, command.UserProjectId);
        
        if (userProject.CreatedUserId != userId ||
            userProject.AllowedUsers.All(au => au.UserId != userId))
        {
            throw new BusinessException(ErrorMessages.Forbidden, 403);
        }

        if (!command.Id.HasValue)
        {
            await this.balanceRepository.AddExpenseAsync(
                this.mapper.Map<Expense>(command), cancellationToken);
            return;
        }
        
        Expense currentExponse = await this.expenseRepository.GetAsync(
            e => e.Id == command.Id.Value, cancellationToken);
        this.entityValidator.ValidateExist(currentExponse, command.Id.Value);
        
        if (currentExponse.UserProjectId != command.UserProjectId)
        {
            throw new BusinessException(ErrorMessages.Forbidden, 403);
        }
        
        await this.balanceRepository.UpdateExpenseAsync(currentExponse,
            this.mapper.Map<Expense>(command), cancellationToken);
    }
    
}