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
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository;
    
    public UpdateUserProjectCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository): base(currentUserRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.userProjectRepository = userProjectRepository;
    }
    
    public async Task Handle(UpdateUserProjectCommand command, CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateVoidRequest<UpdateUserProjectCommand>(command, () => new UpdateUserProjectCommandValidator());
        
        Guid userId = await this.GetCurrentUserIdAsync();
        
        UserProjectEntity userProjectEntity = await this.userProjectRepository.GetByIdAsync(command.Id, cancellationToken);
        
        this.mapper.Map<UpdateUserProjectCommand, UserProjectEntity>(command, userProjectEntity);
        
        await this.userProjectRepository.UpdateAsync(userProjectEntity, cancellationToken);
    }
}