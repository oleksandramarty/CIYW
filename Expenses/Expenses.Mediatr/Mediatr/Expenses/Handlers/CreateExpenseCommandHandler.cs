using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Extensions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Responses.Base;
using Expenses.Business;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Validators.Expenses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class CreateExpenseCommandHandler: MediatrExpensesBase, IRequestHandler<CreateExpenseCommand, BaseEntityIdResponse<Guid>>
{
    private readonly IMapper _mapper;
    private readonly IBalanceRepository _balanceRepository;
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> _readGenericExpenseRepository;
    private readonly IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> _readGenericUserProjectRepository;

    public CreateExpenseCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IBalanceRepository balanceRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> readGenericExpenseRepository,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository
        ) : base(currentUserRepository, entityValidator, readGenericUserProjectRepository)
    {
        _mapper = mapper;
        _balanceRepository = balanceRepository;
        _entityValidator = entityValidator;
        _readGenericExpenseRepository = readGenericExpenseRepository;
        _readGenericUserProjectRepository = readGenericUserProjectRepository;
    }

    public async Task<BaseEntityIdResponse<Guid>> Handle(CreateExpenseCommand command, CancellationToken cancellationToken)
    {        
        _entityValidator.ValidateRequest<CreateExpenseCommand, BaseEntityIdResponse<Guid>>(command, () => new CreateExpenseCommandValidator());

        await CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);

        DateTime currentMonth = DateTimeExtension.GetStartOfCurrentMonth();
        
        if (await _readGenericExpenseRepository.Queryable(fe => 
                    fe.UserProjectId == command.UserProjectId &&
                    fe.CreatedAt >= currentMonth
                    )
                .CountAsync(cancellationToken) >= 50)
        {
            throw new BusinessException(ErrorMessages.UserProjectLimitExceeded, 409);
        }
    
        ExpenseEntity toAdd = _mapper.Map<ExpenseEntity>(command);
        await _balanceRepository.AddExpenseAsync(toAdd, cancellationToken);

        return new BaseEntityIdResponse<Guid>
        {
            Id = toAdd.Id
        };
    }
}