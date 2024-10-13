using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Expenses;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class RemoveUserBalanceCommandHandler: IRequestHandler<RemoveUserBalanceCommand, bool>
{
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, Balance, ExpensesDataContext> balanceRepository;

    public RemoveUserBalanceCommandHandler(
        IEntityValidator<ExpensesDataContext> entityValidator, 
        IGenericRepository<Guid, Balance, ExpensesDataContext> balanceRepository
        )
    {
        this.entityValidator = entityValidator;
        this.balanceRepository = balanceRepository;
    }
    
    public async Task<bool> Handle(RemoveUserBalanceCommand command, CancellationToken cancellationToken)
    {
        Balance balance = await this.balanceRepository.GetByIdAsync(command.Id, cancellationToken);
        this.entityValidator.IsEntityExist(balance);

        await this.balanceRepository.DeleteAsync(balance, cancellationToken);

        return true;
    }
}