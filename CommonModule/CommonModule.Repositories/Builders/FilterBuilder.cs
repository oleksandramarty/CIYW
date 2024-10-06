using System.Linq.Expressions;
using CommonModule.Shared.Common;
using CommonModule.Shared.Requests.Base;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Repositories.Builders;

public class FilterBuilder<TId, T> where T : class
{
    private IQueryable<T> query;
    private long totalCount;

    public FilterBuilder(IQueryable<T> query)
    {
        this.query = query;
    }

    public FilterBuilder<TId, T> ApplyCondition(Expression<Func<T, bool>>? condition)
    {
        if (condition != null)
        {
            query = query.Where(condition);
        }
        totalCount = query.LongCount(); // Calculate and store the total count
        return this;
    }

    public FilterBuilder<TId, T> ApplyQuery(string? queryString)
    {
        if (!string.IsNullOrWhiteSpace(queryString))
        {
            var property = typeof(T).GetProperty("Name");
            if (property != null)
            {
                query = query.Where(x => 
                    EF.Functions.Like(EF.Property<string>(x, "Name"), $"{queryString}%") ||
                    EF.Functions.Like(EF.Property<string>(x, "Name"), $"%{queryString}") ||
                    EF.Functions.Like(EF.Property<string>(x, "Name"), $"%{queryString}%"));
            }
        }
        return this;
    }

    public long GetTotalCount()
    {
        return totalCount;
    }

    public FilterBuilder<TId, T> ApplySort(BaseSortableRequest? sortRequest)
    {
        if (sortRequest != null && !string.IsNullOrWhiteSpace(sortRequest.Column))
        {
            if (string.Equals(sortRequest.Direction, "asc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderBy(x => EF.Property<object>(x, sortRequest.Column));
            }
            else if (string.Equals(sortRequest.Direction, "desc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(x => EF.Property<object>(x, sortRequest.Column));
            }
        }

        return this;
    }

    public FilterBuilder<TId, T> ApplyPagination(PaginatorEntity? paginatorRequest)
    {
        if (paginatorRequest != null)
        {
            if (paginatorRequest.PageNumber < 1)
            {
                paginatorRequest.PageNumber = 1;
            }

            if (paginatorRequest.PageSize < 1)
            {
                paginatorRequest.PageSize = 5;
            }

            if (!paginatorRequest.IsFull)
            {
                query = query
                    .Skip((paginatorRequest.PageNumber - 1) * paginatorRequest.PageSize)
                    .Take(paginatorRequest.PageSize);
            }
        }

        return this;
    }

    public FilterBuilder<TId, T> ApplyDateRangeFilter(BaseDateRangeFilterRequest? dateRangeRequest)
    {
        if (dateRangeRequest != null)
        {
            if (dateRangeRequest.StartDate.HasValue)
            {
                query = query.Where(x => EF.Property<DateTime>(x, "Date") >= dateRangeRequest.StartDate.Value.ToUniversalTime());
            }

            if (dateRangeRequest.EndDate.HasValue)
            {
                query = query.Where(x => EF.Property<DateTime>(x, "Date") <= dateRangeRequest.EndDate.Value.ToUniversalTime());
            }
        }

        return this;
    }

    public IQueryable<T> Build()
    {
        return query;
    }
}