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
    private readonly IMapper _mapper;
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IGenericRepository<Guid, BalanceEntity, ExpensesDataContext> _genericBalanceRepository;
    
    
    public CreateUserBalanceCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, BalanceEntity, ExpensesDataContext> genericBalanceRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository,
        IMapper mapper
        ) : base(currentUserRepository, entityValidator, readGenericUserProjectRepository)
    {
        _mapper = mapper;
        _entityValidator = entityValidator;
        _genericBalanceRepository = genericBalanceRepository;
    }
    
    public async Task<BaseEntityIdResponse<Guid>> Handle(CreateUserBalanceCommand command, CancellationToken cancellationToken)
    {
        _entityValidator.ValidateRequest<CreateUserBalanceCommand, BaseEntityIdResponse<Guid>>(command, () => new CreateUserBalanceCommandValidator());
        
        UserProjectEntity userProjectEntity = await UserProjectByIdAsync(command.UserProjectId, cancellationToken);

        if (userProjectEntity.Balances.Count >= 3)
        {
            throw new BusinessException(ErrorMessages.UserProjectLimitExceeded, 409);
        }
        
        BalanceEntity balanceEntity = _mapper.Map<CreateUserBalanceCommand, BalanceEntity>(command);
        balanceEntity.UserId = await CurrentUserIdAsync();
        balanceEntity.Status = StatusEnum.Active;
        await _genericBalanceRepository.AddAsync(balanceEntity, cancellationToken);

        return new BaseEntityIdResponse<Guid>
        {
            Id = balanceEntity.Id
        };
    }
}