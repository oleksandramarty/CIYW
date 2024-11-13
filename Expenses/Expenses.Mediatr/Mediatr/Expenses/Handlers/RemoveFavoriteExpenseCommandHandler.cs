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
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, FavoriteExpenseEntity, ExpensesDataContext> favoriteExpenseRepository;

    public RemoveFavoriteExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, FavoriteExpenseEntity, ExpensesDataContext> favoriteExpenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository
    ) : base(currentUserRepository, entityValidator, userProjectRepository)
    {
        this.entityValidator = entityValidator;
        this.favoriteExpenseRepository = favoriteExpenseRepository;
    }
    
    public async Task<BaseBoolResponse> Handle(RemoveFavoriteExpenseCommand command, CancellationToken cancellationToken)
    {
        FavoriteExpenseEntity favoriteExpenseEntity = await this.favoriteExpenseRepository.GetByIdAsync(command.Id, cancellationToken);
        this.entityValidator.IsEntityExist(favoriteExpenseEntity);

        await this.CheckUserProjectByIdAsync(favoriteExpenseEntity.UserProjectId, cancellationToken);

        await this.favoriteExpenseRepository.DeleteByIdAsync(command.Id, cancellationToken);

        return new BaseBoolResponse();
    }
}