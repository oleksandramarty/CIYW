using AutoMapper;
using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class UpdateUserBalanceCommandHandler: MediatrExpensesBase, IRequestHandler<UpdateUserBalanceCommand>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IGenericRepository<Guid, Balance, ExpensesDataContext> balanceRepository;
    
    public UpdateUserBalanceCommandHandler(
        IAuthRepository authRepository,
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IGenericRepository<Guid, Balance, ExpensesDataContext> balanceRepository,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
        ) : base(authRepository, entityValidator, userProjectRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.balanceRepository = balanceRepository;
    }
    
    public async Task Handle(UpdateUserBalanceCommand command, CancellationToken cancellationToken)
    {
        await this.CheckUserProjectByIdAsync(command.UserProjectId, cancellationToken);
        
        Balance balance = await this.balanceRepository.GetByIdAsync(command.Id, cancellationToken);
        this.entityValidator.IsEntityExist(balance);
        this.mapper.Map(command, balance, opts => opts.Items["IsUpdate"] = true);
        await this.balanceRepository.UpdateAsync(balance, cancellationToken);
    }
}