using CommonModule.Core.Mediatr;
using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;

namespace Expenses.Mediatr.Mediatr.Expenses.Requests;

public class FilteredPlannedExpensesRequest: MediatrBaseFilteredRequest<PlannedExpenseResponse>
{
    public Guid UserProjectId { get; set; }
    public BaseFilterIdsRequest<int>? CategoryIds { get; set; }
}