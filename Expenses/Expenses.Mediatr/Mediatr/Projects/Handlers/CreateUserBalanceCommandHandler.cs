using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class CreateUserBalanceCommandHandler: MediatrExpensesBase, IRequestHandler<CreateUserBalanceCommand>
{
    private readonly IMapper mapper;
    private readonly IGenericRepository<Guid, BalanceEntity, ExpensesDataContext> balanceRepository;
    
    
    public CreateUserBalanceCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, BalanceEntity, ExpensesDataContext> balanceRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository,
        IMapper mapper
        ) : base(currentUserRepository, entityValidator, userProjectRepository)
    {
        this.mapper = mapper;
        this.balanceRepository = balanceRepository;
    }
    
    public async Task Handle(CreateUserBalanceCommand command, CancellationToken cancellationToken)
    {
        UserProjectEntity userProjectEntity = await this.UserProjectByIdAsync(command.UserProjectId, cancellationToken);

        if (userProjectEntity.Balances.Count >= 3)
        {
            throw new BusinessException(ErrorMessages.UserProjectLimitExceeded, 409);
        }
        
        BalanceEntity balanceEntity = this.mapper.Map<CreateUserBalanceCommand, BalanceEntity>(command);
        balanceEntity.UserId = await this.CurrentUserIdAsync();
        await this.balanceRepository.AddAsync(balanceEntity, cancellationToken);
    }
}