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
    private readonly IGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;
    
    public UpdateUserProjectCommandHandler(
        IAuthRepository authRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository): base(authRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.userProjectRepository = userProjectRepository;
    }
    
    public async Task Handle(UpdateUserProjectCommand command, CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateVoidRequest<UpdateUserProjectCommand>(command, () => new UpdateUserProjectCommandValidator());
        
        Guid userId = await this.GetCurrentUserIdAsync();
        
        UserProject userProject = await this.userProjectRepository.GetByIdAsync(command.Id, cancellationToken);
        
        this.mapper.Map<UpdateUserProjectCommand, UserProject>(command, userProject, opts => opts.Items["IsUpdate"] = true);

        userProject.Version = Guid.NewGuid().ToString("N").ToUpper();
        
        await this.userProjectRepository.UpdateAsync(userProject, cancellationToken);
    }
}