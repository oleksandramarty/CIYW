using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Extensions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Core;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using Expenses.Mediatr.Validators.Projects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class CreateUserProjectCommandHandler: MediatrAuthBase, IRequestHandler<CreateUserProjectCommand, BaseEntityIdResponse<Guid>>
{
    private readonly IMapper _mapper;
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> _genericUserProjectRepository;
    
    public CreateUserProjectCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> genericUserProjectRepository): base(currentUserRepository)
    {
        _mapper = mapper;
        _entityValidator = entityValidator;
        _genericUserProjectRepository = genericUserProjectRepository;
    }
    
    public async Task<BaseEntityIdResponse<Guid>> Handle(CreateUserProjectCommand command, CancellationToken cancellationToken)
    {
        _entityValidator.ValidateRequest<CreateUserProjectCommand, BaseEntityIdResponse<Guid>>(command, () => new CreateUserProjectCommandValidator());
        
        Guid userId = await CurrentUserIdAsync();
        
        if (await _genericUserProjectRepository.Queryable(up => up.CreatedUserId == userId).CountAsync(cancellationToken) >= 3)
        {
            throw new BusinessException(ErrorMessages.UserProjectLimitExceeded, 409);
        }
        
        UserProjectEntity userProjectEntity = _mapper.Map<UserProjectEntity>(command);
        
        userProjectEntity.Id = Guid.NewGuid();
        userProjectEntity.CreatedUserId = userId;
        userProjectEntity.Status = StatusEnum.Active;
        
        await _genericUserProjectRepository.AddAsync(userProjectEntity, cancellationToken);

        return new BaseEntityIdResponse<Guid>
        {
            Id = userProjectEntity.Id
        };
    }
}