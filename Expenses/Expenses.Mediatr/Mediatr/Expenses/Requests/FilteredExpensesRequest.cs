using CommonModule.Core.Mediatr;
using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Requests;

public class FilteredExpensesRequest: MediatrBaseFilteredRequest<ExpenseResponse>
{
    public Guid UserProjectId { get; set; }
    public BaseFilterIdsRequest<int> CategoryIds { get; set; }
}