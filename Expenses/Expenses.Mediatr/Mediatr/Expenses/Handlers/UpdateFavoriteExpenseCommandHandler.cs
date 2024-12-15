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

public class UpdateFavoriteExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<UpdateFavoriteExpenseCommand>
{
    private readonly IMapper _mapper;
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IGenericRepository<Guid, FavoriteExpenseEntity, ExpensesDataContext> _genericFavoriteExpenseRepository;

    public UpdateFavoriteExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, FavoriteExpenseEntity, ExpensesDataContext> genericFavoriteExpenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository
        ) : base(currentUserRepository, entityValidator, readGenericUserProjectRepository)
    {
        _mapper = mapper;
        _entityValidator = entityValidator;
        _genericFavoriteExpenseRepository = genericFavoriteExpenseRepository;
    }

    public async Task Handle(UpdateFavoriteExpenseCommand command, CancellationToken cancellationToken)
    {        
        _entityValidator.ValidateVoidRequest<UpdateFavoriteExpenseCommand>(command, () => new UpdateFavoriteExpenseCommandValidator());
        
        FavoriteExpenseEntity? currentFavoriteExpense = await _genericFavoriteExpenseRepository.Async(
            e => e.Id == command.Id, cancellationToken);
        if (currentFavoriteExpense == null)
        {
            throw new EntityNotFoundException();
        }
        
        await CheckUserProjectByIdAsync(currentFavoriteExpense.UserProjectId, cancellationToken);
        
        await _genericFavoriteExpenseRepository.UpdateAsync(
            _mapper.Map<UpdateFavoriteExpenseCommand, FavoriteExpenseEntity>(command, currentFavoriteExpense), cancellationToken);
    }
}