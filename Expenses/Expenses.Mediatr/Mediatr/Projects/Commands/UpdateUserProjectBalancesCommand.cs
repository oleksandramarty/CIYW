using CommonModule.Shared.Common;
using CommonModule.Shared.Enums.Expenses;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Commands;

public class UpdateUserProjectBalancesCommand: BaseIdEntity<Guid>, IRequest
{
    public List<BalanceDto> Balances { get; set; }
}

public class BalanceDto
{
    public Guid Id { get; set; }
    public int CurrencyId { get; set; }
    public string Title { get; set; }
    public BalanceEnum BalanceType { get; set; }
}