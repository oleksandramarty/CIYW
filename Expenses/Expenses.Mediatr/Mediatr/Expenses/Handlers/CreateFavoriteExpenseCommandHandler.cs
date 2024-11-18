using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Responses.Base;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Validators.Expenses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class CreateFavoriteExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<CreateFavoriteExpenseCommand, BaseEntityIdResponse<Guid>>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, FavoriteExpenseEntity, ExpensesDataContext> favoriteExpenseRepository;

    public CreateFavoriteExpenseCommandHandler(
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

    public async Task<BaseEntityIdResponse<Guid>> Handle(CreateFavoriteExpenseCommand command, CancellationToken cancellationToken)
    {        
        this.entityValidator.ValidateRequest<CreateFavoriteExpenseCommand, BaseEntityIdResponse<Guid>>(command, () => new CreateFavoriteExpenseCommandValidator());

        await this.CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);

        if (await this.favoriteExpenseRepository.Queryable(fe => fe.UserProjectId == command.UserProjectId)
                .CountAsync(cancellationToken) >= 10)
        {
            throw new BusinessException(ErrorMessages.UserProjectLimitExceeded, 409);
        }

        FavoriteExpenseEntity toAdd = this.mapper.Map<FavoriteExpenseEntity>(command);
        toAdd.CreatedUserId = await this.CurrentUserIdAsync();
            
        await this.favoriteExpenseRepository.AddAsync(toAdd, cancellationToken);

        return new BaseEntityIdResponse<Guid>
        {
            Id = toAdd.Id
        };
    }
}