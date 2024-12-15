using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using Expenses.Mediatr.Validators.Projects;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class UpdateUserBalanceCommandHandler: MediatrExpensesBase, IRequestHandler<UpdateUserBalanceCommand>
{
    private readonly IMapper _mapper;
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IGenericRepository<Guid, BalanceEntity, ExpensesDataContext> _genericBalanceRepository;
    
    public UpdateUserBalanceCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, BalanceEntity, ExpensesDataContext> genericBalanceRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository
        ) : base(currentUserRepository, entityValidator, readGenericUserProjectRepository)
    {
        _mapper = mapper;
        _entityValidator = entityValidator;
        _genericBalanceRepository = genericBalanceRepository;
    }
    
    public async Task Handle(UpdateUserBalanceCommand command, CancellationToken cancellationToken)
    {
        _entityValidator.ValidateVoidRequest<UpdateUserBalanceCommand>(command, () => new UpdateUserBalanceCommandValidator());
        
        await CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);
        
        BalanceEntity? balance = await _genericBalanceRepository.ByIdAsync(command.Id, cancellationToken);
        if (balance == null)
        {
            throw new EntityNotFoundException();
        }
        
        _mapper.Map(command, balance);
        await _genericBalanceRepository.UpdateAsync(balance, cancellationToken);
    }
}