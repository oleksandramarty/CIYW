using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Requests;

public class GetFilteredFavoriteExpensesRequest: BaseFilterRequest, IRequest<FilteredListResponse<FavoriteExpenseResponse>>
{
    public Guid UserProjectId { get; set; }
    public BaseFilterIdsRequest<int> CategoryIds { get; set; }
}