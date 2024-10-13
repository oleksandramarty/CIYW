using AutoMapper;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class CreateUserBalanceCommandHandler: MediatrExpensesBase, IRequestHandler<CreateUserBalanceCommand>
{
    private readonly IMapper mapper;
    private readonly IGenericRepository<Guid, Balance, ExpensesDataContext> balanceRepository;
    
    
    public CreateUserBalanceCommandHandler(
        IAuthRepository authRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, Balance, ExpensesDataContext> balanceRepository,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository,
        IMapper mapper
        ) : base(authRepository, entityValidator, userProjectRepository)
    {
        this.mapper = mapper;
        this.balanceRepository = balanceRepository;
    }
    
    public async Task Handle(CreateUserBalanceCommand command, CancellationToken cancellationToken)
    {
        await this.CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);
        
        Balance balance = this.mapper.Map<CreateUserBalanceCommand, Balance>(command, opts => opts.Items["IsUpdate"] = false);
        balance.UserId = await this.GetCurrentUserIdAsync();
        await this.balanceRepository.AddAsync(balance, cancellationToken);
    }
}