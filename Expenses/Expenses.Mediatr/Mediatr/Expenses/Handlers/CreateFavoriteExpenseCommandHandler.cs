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
    private readonly IMapper _mapper;
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IGenericRepository<Guid, FavoriteExpenseEntity, ExpensesDataContext> _genericFavoriteExpenseRepository;

    public CreateFavoriteExpenseCommandHandler(
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

    public async Task<BaseEntityIdResponse<Guid>> Handle(CreateFavoriteExpenseCommand command, CancellationToken cancellationToken)
    {        
        _entityValidator.ValidateRequest<CreateFavoriteExpenseCommand, BaseEntityIdResponse<Guid>>(command, () => new CreateFavoriteExpenseCommandValidator());

        await CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);

        if (await _genericFavoriteExpenseRepository.Queryable(fe => fe.UserProjectId == command.UserProjectId)
                .CountAsync(cancellationToken) >= 10)
        {
            throw new BusinessException(ErrorMessages.UserProjectLimitExceeded, 409);
        }

        FavoriteExpenseEntity toAdd = _mapper.Map<FavoriteExpenseEntity>(command);
        toAdd.CreatedUserId = await CurrentUserIdAsync();
            
        await _genericFavoriteExpenseRepository.AddAsync(toAdd, cancellationToken);

        return new BaseEntityIdResponse<Guid>
        {
            Id = toAdd.Id
        };
    }
}