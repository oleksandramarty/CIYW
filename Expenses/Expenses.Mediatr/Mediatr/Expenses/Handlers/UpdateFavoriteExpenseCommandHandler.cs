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
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, FavoriteExpenseEntity, ExpensesDataContext> favoriteExpenseRepository;

    public UpdateFavoriteExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, FavoriteExpenseEntity, ExpensesDataContext> favoriteExpenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository
        ) : base(currentUserRepository, entityValidator, userProjectRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.favoriteExpenseRepository = favoriteExpenseRepository;
    }

    public async Task Handle(UpdateFavoriteExpenseCommand command, CancellationToken cancellationToken)
    {        
        this.entityValidator.ValidateVoidRequest<UpdateFavoriteExpenseCommand>(command, () => new UpdateFavoriteExpenseCommandValidator());
        
        FavoriteExpenseEntity? currentFavoriteExpense = await this.favoriteExpenseRepository.Async(
            e => e.Id == command.Id, cancellationToken);
        if (currentFavoriteExpense == null)
        {
            throw new EntityNotFoundException();
        }
        
        await this.CheckUserProjectByIdAsync(currentFavoriteExpense.UserProjectId, cancellationToken);
        
        await this.favoriteExpenseRepository.UpdateAsync(
            this.mapper.Map<UpdateFavoriteExpenseCommand, FavoriteExpenseEntity>(command, currentFavoriteExpense), cancellationToken);
    }
}