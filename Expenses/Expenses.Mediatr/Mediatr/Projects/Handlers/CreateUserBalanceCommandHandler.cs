using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using Expenses.Mediatr.Validators.Projects;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class CreateUserBalanceCommandHandler: MediatrExpensesBase, IRequestHandler<CreateUserBalanceCommand, BaseEntityIdResponse<Guid>>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
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
        this.entityValidator = entityValidator;
        this.balanceRepository = balanceRepository;
    }
    
    public async Task<BaseEntityIdResponse<Guid>> Handle(CreateUserBalanceCommand command, CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateRequest<CreateUserBalanceCommand, BaseEntityIdResponse<Guid>>(command, () => new CreateUserBalanceCommandValidator());
        
        UserProjectEntity userProjectEntity = await this.UserProjectByIdAsync(command.UserProjectId, cancellationToken);

        if (userProjectEntity.Balances.Count >= 3)
        {
            throw new BusinessException(ErrorMessages.UserProjectLimitExceeded, 409);
        }
        
        BalanceEntity balanceEntity = this.mapper.Map<CreateUserBalanceCommand, BalanceEntity>(command);
        balanceEntity.UserId = await this.CurrentUserIdAsync();
        balanceEntity.Status = StatusEnum.Active;
        await this.balanceRepository.AddAsync(balanceEntity, cancellationToken);

        return new BaseEntityIdResponse<Guid>
        {
            Id = balanceEntity.Id
        };
    }
}