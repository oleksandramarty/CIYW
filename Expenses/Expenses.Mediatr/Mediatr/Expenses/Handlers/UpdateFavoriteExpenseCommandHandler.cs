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
    private readonly IGenericRepository<Guid, FavoriteExpense, ExpensesDataContext> favoriteExpenseRepository;

    public UpdateFavoriteExpenseCommandHandler(
        IAuthRepository authRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, FavoriteExpense, ExpensesDataContext> favoriteExpenseRepository,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
        ) : base(authRepository, entityValidator, userProjectRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.favoriteExpenseRepository = favoriteExpenseRepository;
    }

    public async Task Handle(UpdateFavoriteExpenseCommand command, CancellationToken cancellationToken)
    {        
        this.entityValidator.ValidateVoidRequest<UpdateFavoriteExpenseCommand>(command, () => new UpdateFavoriteExpenseCommandValidator());
        
        FavoriteExpense currentFavoriteExpense = await this.favoriteExpenseRepository.GetAsync(
            e => e.Id == command.Id, cancellationToken);
        this.entityValidator.IsEntityExist(currentFavoriteExpense);
        
        await this.CheckUserProjectByIdAsync(currentFavoriteExpense.UserProjectId, cancellationToken);
        
        await this.favoriteExpenseRepository.UpdateAsync(
            this.mapper.Map<UpdateFavoriteExpenseCommand, FavoriteExpense>(command, currentFavoriteExpense, opts => opts.Items["IsUpdate"] = true), cancellationToken);
    }
}