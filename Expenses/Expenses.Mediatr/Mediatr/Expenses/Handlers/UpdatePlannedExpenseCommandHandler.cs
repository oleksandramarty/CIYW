using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Validators.Expenses;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class UpdatePlannedExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<UpdatePlannedExpenseCommand>
{
    private readonly IMapper _mapper;
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IGenericRepository<Guid, PlannedExpenseEntity, ExpensesDataContext> _readGenericPlannedExpenseRepository;

    public UpdatePlannedExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, PlannedExpenseEntity, ExpensesDataContext> readGenericPlannedExpenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository
        ) : base(currentUserRepository, entityValidator, readGenericUserProjectRepository)
    {
        _mapper = mapper;
        _entityValidator = entityValidator;
        _readGenericPlannedExpenseRepository = readGenericPlannedExpenseRepository;
    }

    public async Task Handle(UpdatePlannedExpenseCommand command, CancellationToken cancellationToken)
    {        
        _entityValidator.ValidateVoidRequest<UpdatePlannedExpenseCommand>(command, () => new UpdatePlannedExpenseCommandValidator());
        
        PlannedExpenseEntity? currentPlannedExpense = await _readGenericPlannedExpenseRepository.Async(
            e => e.Id == command.Id, cancellationToken);
        if (currentPlannedExpense == null)
        {
            throw new EntityNotFoundException();
        }
        
        await CheckUserProjectByIdAsync(currentPlannedExpense.UserProjectId, cancellationToken);
        
        await _readGenericPlannedExpenseRepository.UpdateAsync(
            _mapper.Map<UpdatePlannedExpenseCommand, PlannedExpenseEntity>(command, currentPlannedExpense), cancellationToken);
    }
}