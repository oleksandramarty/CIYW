using AutoMapper;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Categories.Commands;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Categories.Handlers;

public class CreateUserProjectCommandHandler: MediatrAuthBase, IRequestHandler<CreateUserProjectCommand>
{
    private readonly IMapper mapper;
    private readonly IGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    
    public CreateUserProjectCommandHandler(
        IAuthRepository authRepository,
        IMapper mapper,
        IGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository,
        IEntityValidator<ExpensesDataContext> entityValidator): base(authRepository)
    {
        this.mapper = mapper;
        this.userProjectRepository = userProjectRepository;
        this.entityValidator = entityValidator;
    }
    
    public async Task Handle(CreateUserProjectCommand command, CancellationToken cancellationToken)
    {
        UserProject userProject = this.mapper.Map<UserProject>(command);
        
        userProject.CreatedUserId = await this.GetUserIdAsync();
        
        await this.userProjectRepository.AddAsync(userProject, cancellationToken);
    }
}