using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Extensions;
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

public class CreatePlannedExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<CreatePlannedExpenseCommand, BaseEntityIdResponse<Guid>>
{
    private readonly IMapper _mapper;
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IGenericRepository<Guid, PlannedExpenseEntity, ExpensesDataContext> _readGenericPlannedExpenseRepository;

    public CreatePlannedExpenseCommandHandler(
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

    public async Task<BaseEntityIdResponse<Guid>> Handle(CreatePlannedExpenseCommand command, CancellationToken cancellationToken)
    {        
        _entityValidator.ValidateRequest<CreatePlannedExpenseCommand, BaseEntityIdResponse<Guid>>(command, () => new CreatePlannedExpenseCommandValidator());

        await CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);
        
        if (await _readGenericPlannedExpenseRepository.Queryable(fe => fe.UserProjectId == command.UserProjectId)
                .CountAsync(cancellationToken) >= 10)
        {
            throw new BusinessException(ErrorMessages.UserProjectLimitExceeded, 409);
        }

        PlannedExpenseEntity toAdd = _mapper.Map<PlannedExpenseEntity>(command);
        toAdd.CreatedUserId = await CurrentUserIdAsync();
            
        await _readGenericPlannedExpenseRepository.AddAsync(toAdd, cancellationToken);

        return new BaseEntityIdResponse<Guid>
        {
            Id = toAdd.Id
        };
    }
}