using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using Expenses.Mediatr.Validators.Projects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class UpdateUserProjectCommandHandler: MediatrAuthBase, IRequestHandler<UpdateUserProjectCommand>
{
    private readonly IMapper _mapper;
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> _genericUserProjectRepository;
    
    public UpdateUserProjectCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> genericUserProjectRepository): base(currentUserRepository)
    {
        _mapper = mapper;
        _entityValidator = entityValidator;
        _genericUserProjectRepository = genericUserProjectRepository;
    }
    
    public async Task Handle(UpdateUserProjectCommand command, CancellationToken cancellationToken)
    {
        _entityValidator.ValidateVoidRequest<UpdateUserProjectCommand>(command, () => new UpdateUserProjectCommandValidator());
        
        Guid userId = await CurrentUserIdAsync();
        
        UserProjectEntity userProjectEntity = await _genericUserProjectRepository.ByIdAsync(command.Id, cancellationToken);
        
        _mapper.Map<UpdateUserProjectCommand, UserProjectEntity>(command, userProjectEntity);
        
        await _genericUserProjectRepository.UpdateAsync(userProjectEntity, cancellationToken);
    }
}