using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class RemoveFavoriteExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<RemoveFavoriteExpenseCommand, BaseBoolResponse>
{
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IGenericRepository<Guid, FavoriteExpenseEntity, ExpensesDataContext> _genericFavoriteExpenseRepository;

    public RemoveFavoriteExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, FavoriteExpenseEntity, ExpensesDataContext> genericFavoriteExpenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository
    ) : base(currentUserRepository, entityValidator, readGenericUserProjectRepository)
    {
        _entityValidator = entityValidator;
        _genericFavoriteExpenseRepository = genericFavoriteExpenseRepository;
    }
    
    public async Task<BaseBoolResponse> Handle(RemoveFavoriteExpenseCommand command, CancellationToken cancellationToken)
    {
        FavoriteExpenseEntity? favoriteExpense = await _genericFavoriteExpenseRepository.ByIdAsync(command.Id, cancellationToken);
        if (favoriteExpense == null)
        {
            throw new EntityNotFoundException();
        }

        await CheckUserProjectByIdAsync(favoriteExpense.UserProjectId, cancellationToken);

        await _genericFavoriteExpenseRepository.DeleteByIdAsync(command.Id, cancellationToken);

        return new BaseBoolResponse();
    }
}