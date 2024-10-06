using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Requests;

public class GetFilteredPlannedExpensesRequest: BaseFilterRequest, IRequest<ListWithIncludeResponse<PlannedExpenseResponse>>
{
    public Guid UserProjectId { get; set; }
    public List<int> CategoryIds { get; set; }
}