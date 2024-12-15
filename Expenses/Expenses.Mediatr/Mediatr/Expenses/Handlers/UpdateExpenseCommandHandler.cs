using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using Expenses.Business;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Validators.Expenses;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class UpdateExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<UpdateExpenseCommand>
{
    private readonly IMapper _mapper;
    private readonly IBalanceRepository _balanceRepository;
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> _readGenericExpenseRepository;
    
    public UpdateExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IBalanceRepository balanceRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> readGenericExpenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository
        ) : base(currentUserRepository, entityValidator, readGenericUserProjectRepository)
    {
        _mapper = mapper;
        _balanceRepository = balanceRepository;
        _entityValidator = entityValidator;
        _readGenericExpenseRepository = readGenericExpenseRepository;
    }

    public async Task Handle(UpdateExpenseCommand command, CancellationToken cancellationToken)
    {        
        _entityValidator.ValidateVoidRequest<UpdateExpenseCommand>(command, () => new UpdateExpenseCommandValidator());

        ExpenseEntity? currentExpense = await _readGenericExpenseRepository.Async(
            e => e.Id == command.Id, cancellationToken);
        if (currentExpense == null)
        {
            throw new EntityNotFoundException();
        }
        
        await CheckUserProjectByIdAsync(currentExpense.UserProjectId, cancellationToken);
        
        await _balanceRepository.UpdateExpenseAsync(currentExpense,
            _mapper.Map<ExpenseEntity>(command), cancellationToken);
    }
}