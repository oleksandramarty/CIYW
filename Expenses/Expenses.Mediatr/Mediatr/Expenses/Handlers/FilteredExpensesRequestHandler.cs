using CommonModule.Core.Extensions;
using CommonModule.Core.Strategies.FilteredResult;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using Expenses.Domain;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class FilteredExpensesRequestHandler : MediatrExpensesBase,
    IRequestHandler<FilteredExpensesRequest, FilteredListResponse<ExpenseResponse>>
{
    private readonly IFilteredResultStrategy<FilteredExpensesRequest, ExpenseResponse> _strategy;

    public FilteredExpensesRequestHandler(
        ICurrentUserRepository currentUserRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository,
        IFilteredResultStrategy<FilteredExpensesRequest, ExpenseResponse> strategy
    ) : base(currentUserRepository, entityValidator, readGenericUserProjectRepository)
    {
        _strategy = strategy;
    }

    public async Task<FilteredListResponse<ExpenseResponse>> Handle(FilteredExpensesRequest request,
        CancellationToken cancellationToken)
    {
        await CheckUserProjectByIdAsync(request.UserProjectId, cancellationToken);

        request.CheckBaseFilter();
        request.CategoryIds.CheckIds();

        return await _strategy.FilteredResultAsync(request, cancellationToken);
    }
}