using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class RemoveFavoriteExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<RemoveFavoriteExpenseCommand, bool>
{
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, FavoriteExpense, ExpensesDataContext> favoriteExpenseRepository;

    public RemoveFavoriteExpenseCommandHandler(
        IAuthRepository authRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, FavoriteExpense, ExpensesDataContext> favoriteExpenseRepository,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
    ) : base(authRepository, entityValidator, userProjectRepository)
    {
        this.entityValidator = entityValidator;
        this.favoriteExpenseRepository = favoriteExpenseRepository;
    }
    
    public async Task<bool> Handle(RemoveFavoriteExpenseCommand command, CancellationToken cancellationToken)
    {
        FavoriteExpense favoriteExpense = await this.favoriteExpenseRepository.GetByIdAsync(command.Id, cancellationToken);
        this.entityValidator.IsEntityExist(favoriteExpense);

        await this.CheckUserProjectByIdAsync(favoriteExpense.UserProjectId, cancellationToken);

        await this.favoriteExpenseRepository.DeleteByIdAsync(command.Id, cancellationToken);

        return true;
    }
}