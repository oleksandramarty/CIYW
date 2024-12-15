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

public class FilteredResultOfFavoriteExpenseStrategy: FilteredResultStrategyResponseContext<FilteredFavoriteExpensesRequest, FavoriteExpenseEntity, FavoriteExpenseResponse>, IFilteredResultStrategy<FilteredFavoriteExpensesRequest, FavoriteExpenseResponse>
{
    private readonly IReadGenericRepository<Guid, FavoriteExpenseEntity, ExpensesDataContext> _readGenericFavoriteExpenseRepository;

    public FilteredResultOfFavoriteExpenseStrategy(
        IMapper mapper,
        IReadGenericRepository<Guid, FavoriteExpenseEntity, ExpensesDataContext> readGenericFavoriteExpenseRepository
        ): base(mapper)
    {
        _readGenericFavoriteExpenseRepository = readGenericFavoriteExpenseRepository;
    }

    public async Task<FilteredListResponse<FavoriteExpenseResponse>> FilteredResultAsync(FilteredFavoriteExpensesRequest request, CancellationToken cancellationToken)
    {
        if (request.CategoryIds == null)
        {
            request.CategoryIds = new BaseFilterIdsRequest<int>();
        }
        
        var query = _readGenericFavoriteExpenseRepository.Queryable(
            e => e.UserProjectId == request.UserProjectId &&
                 (string.IsNullOrEmpty(request.Query) || EF.Functions.Like(e.Title, $"%{request.Query}%")) &&
                 (!request.CategoryIds.Ids.Any() || e.CategoryId.HasValue && request.CategoryIds.Ids.Contains(e.CategoryId.Value)) &&
                 (request.DateRange == null || (
                         request.DateRange.StartDate.HasValue && !request.DateRange.EndDate.HasValue && e.EndDate >= request.DateRange.StartDate && e.EndDate.HasValue ||
                         !request.DateRange.StartDate.HasValue && request.DateRange.EndDate.HasValue && e.EndDate <= request.DateRange.EndDate && e.EndDate.HasValue ||
                         request.DateRange.StartDate.HasValue && request.DateRange.EndDate.HasValue && e.EndDate >= request.DateRange.StartDate && e.EndDate <= request.DateRange.EndDate && e.EndDate.HasValue
                         )
                     ) &&
                 (
                     request.AmountRange == null || (
                         request.AmountRange.AmountFrom.HasValue && !request.AmountRange.AmountTo.HasValue && e.Limit >= request.AmountRange.AmountFrom ||
                         !request.AmountRange.AmountFrom.HasValue && request.AmountRange.AmountTo.HasValue && e.Limit <= request.AmountRange.AmountTo ||
                         request.AmountRange.AmountFrom.HasValue && request.AmountRange.AmountTo.HasValue && e.Limit >= request.AmountRange.AmountFrom && e.Limit <= request.AmountRange.AmountTo
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
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Limit) : query.OrderByDescending(x => x.Limit);
                    break;
                case ColumnEnum.CurrentAmount:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.CurrentAmount) : query.OrderByDescending(x => x.CurrentAmount);
                    break;
                case ColumnEnum.CreatedAt:
                    query.SortByCreatedAt(request.Sort.Direction);
                    break;
                case ColumnEnum.UpdatedAt:
                    query.SortByUpdatedAt(request.Sort.Direction);
                    break;
                default:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.EndDate) : query.OrderByDescending(x => x.EndDate);
                    break;
            }
        }
        
        return await FilteredResultAsync(request, query, cancellationToken);
    }
}