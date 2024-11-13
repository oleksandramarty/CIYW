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

public class CreateUserProjectCommandHandler: MediatrAuthBase, IRequestHandler<CreateUserProjectCommand>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository;
    
    public CreateUserProjectCommandHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository): base(currentUserRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.userProjectRepository = userProjectRepository;
    }
    
    public async Task Handle(CreateUserProjectCommand command, CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateVoidRequest<CreateUserProjectCommand>(command, () => new CreateUserProjectCommandValidator());
        
        Guid userId = await this.GetCurrentUserIdAsync();
        
        UserProjectEntity userProjectEntity = this.mapper.Map<UserProjectEntity>(command);
        
        userProjectEntity.Id = Guid.NewGuid();
        userProjectEntity.CreatedUserId = userId;
        userProjectEntity.Version = Guid.NewGuid().ToString("N").ToUpper();
        
        await this.userProjectRepository.AddAsync(userProjectEntity, cancellationToken);
    }
}