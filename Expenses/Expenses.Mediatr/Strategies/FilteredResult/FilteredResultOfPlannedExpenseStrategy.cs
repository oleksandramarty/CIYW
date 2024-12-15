using AutoMapper;
using CommonModule.Core.Extensions;
using CommonModule.Core.Strategies.FilteredResult;
using CommonModule.Interfaces;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Strategies.FilteredResult;

public class FilteredResultOfPlannedExpenseStrategy: FilteredResultStrategyResponseContext<FilteredPlannedExpensesRequest, PlannedExpenseEntity, PlannedExpenseResponse>, IFilteredResultStrategy<FilteredPlannedExpensesRequest, PlannedExpenseResponse>
{
    private readonly IReadGenericRepository<Guid, PlannedExpenseEntity, ExpensesDataContext> _readGenericPlannedExpenseRepository;

    public FilteredResultOfPlannedExpenseStrategy(
        IMapper mapper,
        IReadGenericRepository<Guid, PlannedExpenseEntity, ExpensesDataContext> readGenericPlannedExpenseRepository
        ): base(mapper)
    {
        _readGenericPlannedExpenseRepository = readGenericPlannedExpenseRepository;
    }

    public async Task<FilteredListResponse<PlannedExpenseResponse>> FilteredResultAsync(FilteredPlannedExpensesRequest request, CancellationToken cancellationToken)
    {
        if (request.CategoryIds == null)
        {
            request.CategoryIds = new BaseFilterIdsRequest<int>();
        }
        
        var query = _readGenericPlannedExpenseRepository.Queryable(
            e => e.UserProjectId == request.UserProjectId &&
                 (string.IsNullOrEmpty(request.Query) || EF.Functions.Like(e.Title, $"%{request.Query}%")) &&
                 (!request.CategoryIds.Ids.Any() || request.CategoryIds.Ids.Contains(e.CategoryId)) &&
                 (request.DateRange == null || (
                         request.DateRange.StartDate.HasValue && !request.DateRange.EndDate.HasValue && e.NextDate >= request.DateRange.StartDate ||
                         !request.DateRange.StartDate.HasValue && request.DateRange.EndDate.HasValue && e.NextDate <= request.DateRange.EndDate ||
                         request.DateRange.StartDate.HasValue && request.DateRange.EndDate.HasValue && e.NextDate >= request.DateRange.StartDate && e.NextDate <= request.DateRange.EndDate
                         )
                     ) &&
                 (
                     request.AmountRange == null || (
                         request.AmountRange.AmountFrom.HasValue && !request.AmountRange.AmountTo.HasValue && e.Amount >= request.AmountRange.AmountFrom ||
                         !request.AmountRange.AmountFrom.HasValue && request.AmountRange.AmountTo.HasValue && e.Amount <= request.AmountRange.AmountTo ||
                         request.AmountRange.AmountFrom.HasValue && request.AmountRange.AmountTo.HasValue && e.Amount >= request.AmountRange.AmountFrom && e.Amount <= request.AmountRange.AmountTo
                         )
                     )
            );

        if (request.Sort != null && request.Sort.Column.HasValue)
        {
            switch (request.Sort.Column.Value)
            {
                case ColumnEnum.Title:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Title) : query.OrderByDescending(x => x.Title);
                    break;
                case ColumnEnum.Amount:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Amount) : query.OrderByDescending(x => x.Amount);
                    break;
                case ColumnEnum.CreatedAt:
                    query.SortByCreatedAt(request.Sort.Direction);
                    break;
                case ColumnEnum.UpdatedAt:
                    query.SortByUpdatedAt(request.Sort.Direction);
                    break;
                default:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.NextDate) : query.OrderByDescending(x => x.NextDate);
                    break;
            }
        }
        
        return await FilteredResultAsync(request, query, cancellationToken);
    }
}