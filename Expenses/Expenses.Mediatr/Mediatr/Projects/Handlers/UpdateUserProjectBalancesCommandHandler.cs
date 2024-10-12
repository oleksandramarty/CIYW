using AutoMapper;
using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class UpdateUserProjectBalancesCommandHandler: IRequestHandler<UpdateUserProjectBalancesCommand>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;
    private readonly IGenericRepository<Guid, Balance, ExpensesDataContext> balanceRepository;
    
    public UpdateUserProjectBalancesCommandHandler(
        IMapper mapper,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository,
        IGenericRepository<Guid, Balance, ExpensesDataContext> balanceRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.userProjectRepository = userProjectRepository;
        this.balanceRepository = balanceRepository;
    }
    
    public async Task Handle(UpdateUserProjectBalancesCommand command, CancellationToken cancellationToken)
    {
        List<Balance> existingBalances = await this.balanceRepository.GetListAsync(b => b.UserProjectId == command.Id, cancellationToken);

        List<Balance> newBalances = new List<Balance>();

        foreach (var balance in command.Balances)
        {
            var existingBalance = existingBalances.FirstOrDefault(b => b.Id == balance.Id);
            if (existingBalance == null)
            {
                newBalances.Add(this.mapper.Map<BalanceDto, Balance>(balance, opts => opts.Items["IsUpdate"] = false));
            }
            else
            {
                existingBalance = this.mapper.Map<BalanceDto, Balance>(balance, opts => opts.Items["IsUpdate"] = true);
            }
        }

        if (existingBalances.Any())
        {
            await this.balanceRepository.UpdateRangeAsync(existingBalances, cancellationToken);
        }
        
        if (newBalances.Any())
        {
            await this.balanceRepository.AddRangeAsync(newBalances, cancellationToken);
        }
    }
}