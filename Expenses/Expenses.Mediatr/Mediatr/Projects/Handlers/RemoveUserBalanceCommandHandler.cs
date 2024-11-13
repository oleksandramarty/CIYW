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
        BalanceEntity balanceEntity = await this.balanceRepository.GetByIdAsync(command.Id, cancellationToken);
        this.entityValidator.IsEntityExist(balanceEntity);

        await this.balanceRepository.DeleteAsync(balanceEntity, cancellationToken);

        return new BaseBoolResponse();
    }
}