using CommonModule.Shared.Common;
using CommonModule.Shared.Responses.Base;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Commands;

public class CreateFavoriteExpenseCommand: IRequest<BaseEntityIdResponse<Guid>>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal? Limit { get; set; }
    public int? CategoryId { get; set; }
    public int? FrequencyId { get; set; }
    public int CurrencyId { get; set; }
    public Guid UserProjectId { get; set; }
    public int IconId { get; set; }
}