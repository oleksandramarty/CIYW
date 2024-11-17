using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Expenses;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class RemoveUserBalanceCommandHandler: IRequestHandler<RemoveUserBalanceCommand, BaseBoolResponse>
{
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, BalanceEntity, ExpensesDataContext> balanceRepository;

    public RemoveUserBalanceCommandHandler(
        IEntityValidator<ExpensesDataContext> entityValidator, 
        IGenericRepository<Guid, BalanceEntity, ExpensesDataContext> balanceRepository
        )
    {
        this.entityValidator = entityValidator;
        this.balanceRepository = balanceRepository;
    }
    
    public async Task<BaseBoolResponse> Handle(RemoveUserBalanceCommand command, CancellationToken cancellationToken)
    {
        BalanceEntity? balance = await this.balanceRepository.ByIdAsync(command.Id, cancellationToken);
        if (balance == null)
        {
            throw new EntityNotFoundException();
        }

        await this.balanceRepository.DeleteAsync(balance, cancellationToken);

        return new BaseBoolResponse();
    }
}