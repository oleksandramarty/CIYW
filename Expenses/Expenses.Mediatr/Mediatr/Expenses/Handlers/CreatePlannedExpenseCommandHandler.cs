using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Extensions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Validators.Expenses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class CreatePlannedExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<CreatePlannedExpenseCommand>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, PlannedExpenseEntity, ExpensesDataContext> plannedExpenseRepository;

    public CreatePlannedExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, PlannedExpenseEntity, ExpensesDataContext> plannedExpenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository
        ) : base(currentUserRepository, entityValidator, userProjectRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.plannedExpenseRepository = plannedExpenseRepository;
    }

    public async Task Handle(CreatePlannedExpenseCommand command, CancellationToken cancellationToken)
    {        
        this.entityValidator.ValidateVoidRequest<CreatePlannedExpenseCommand>(command, () => new CreatePlannedExpenseCommandValidator());

        await this.CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);
        
        if (await this.plannedExpenseRepository.GetQueryable(fe => fe.UserProjectId == command.UserProjectId)
                .CountAsync(cancellationToken) >= 10)
        {
            throw new BusinessException(ErrorMessages.UserProjectLimitExceeded, 409);
        }

        PlannedExpenseEntity toAdd = this.mapper.Map<PlannedExpenseEntity>(command);
            
        await this.plannedExpenseRepository.AddAsync(toAdd, cancellationToken);
        return;
    }
}