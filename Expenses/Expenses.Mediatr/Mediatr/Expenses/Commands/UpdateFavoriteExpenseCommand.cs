using CommonModule.Shared.Common;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Commands;

public class UpdateFavoriteExpenseCommand: BaseIdEntity<Guid>, IRequest
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal CurrentAmount { get; set; }
    public decimal Limit { get; set; }
    public Guid BalanceId { get; set; }
    public int CategoryId { get; set; }
    public int FrequencyId { get; set; }
    public Guid UserProjectId { get; set; }
    public int IconId { get; set; }
}