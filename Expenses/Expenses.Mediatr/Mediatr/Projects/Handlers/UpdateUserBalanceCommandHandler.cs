using AutoMapper;
using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class UpdateUserBalanceCommandHandler: MediatrExpensesBase, IRequestHandler<UpdateUserBalanceCommand>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, BalanceEntity, ExpensesDataContext> balanceRepository;
    
    public UpdateUserBalanceCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, BalanceEntity, ExpensesDataContext> balanceRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository
        ) : base(currentUserRepository, entityValidator, userProjectRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.balanceRepository = balanceRepository;
    }
    
    public async Task Handle(UpdateUserBalanceCommand command, CancellationToken cancellationToken)
    {
        await this.CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);
        
        BalanceEntity balanceEntity = await this.balanceRepository.GetByIdAsync(command.Id, cancellationToken);
        this.entityValidator.IsEntityExist(balanceEntity);
        this.mapper.Map(command, balanceEntity);
        await this.balanceRepository.UpdateAsync(balanceEntity, cancellationToken);
    }
}