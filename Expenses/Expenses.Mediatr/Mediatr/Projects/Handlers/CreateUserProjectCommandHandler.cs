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
    private readonly IGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;
    
    public CreateUserProjectCommandHandler(
        IAuthRepository authRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository): base(authRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.userProjectRepository = userProjectRepository;
    }
    
    public async Task Handle(CreateUserProjectCommand command, CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateVoidRequest<CreateUserProjectCommand>(command, () => new CreateUserProjectCommandValidator());
        
        Guid userId = await this.GetCurrentUserIdAsync();
        int count = await this.userProjectRepository.GetQueryable(
            p => p.CreatedUserId == userId).CountAsync(cancellationToken);

        if (count > 0)
        {
            throw new BusinessException(ErrorMessages.UserProjectLimitExceeded, 409);
        }
        
        UserProject userProject = this.mapper.Map<UserProject>(command, opts => opts.Items["IsUpdate"] = false);
        
        userProject.Id = Guid.NewGuid();
        userProject.CreatedUserId = userId;
        userProject.Version = Guid.NewGuid().ToString("N").ToUpper();

        userProject.Balances = command.CurrencyIds.Select(c => new Balance
        {
            Id = Guid.NewGuid(),
            Amount = 0,
            Created = DateTime.UtcNow,
            CurrencyId = c,
            UserProjectId = userProject.Id,
            UserId = userId,
            Version = Guid.NewGuid().ToString("N").ToUpper()
        }).ToList();
        
        await this.userProjectRepository.AddAsync(userProject, cancellationToken);
    }
}