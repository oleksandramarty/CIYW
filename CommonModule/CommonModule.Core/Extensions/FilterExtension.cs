using CommonModule.Shared.Common;
using CommonModule.Shared.Requests.Base;

namespace CommonModule.Core.Extensions;

public static class FilterExtension
{
    public static void CheckBaseFilter(this BaseFilterRequest filter)
    {
        if (filter == null)
        {
            throw new ArgumentNullException(nameof(filter));
        }
        
        filter.Query ??= string.Empty;
        filter.DateRange.CheckOrApplyDefaultExpenseFilter();
        filter.AmountRange.CheckAmountRangeFilter();
        filter.Paginator.CheckPaginator();
    }
    public static void CheckOrApplyDefaultExpenseFilter(this BaseDateRangeFilterRequest? range)
    {
        if (range == null)
        {
            range = new BaseDateRangeFilterRequest()
            {
                StartDate = DateTimeExtension.GetStartOfCurrentMonth()
            };
        }
        else
        {
            range.StartDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        range.StartDate.SetMidnight();
        range.EndDate.SetMidnight();
    }
    
    public static void CheckAmountRangeFilter(this BaseAmountRangeFilterRequest? range)
    {
        if (range != null)
        {
            if (range.AmountFrom.HasValue && range.AmountTo.HasValue && range.AmountFrom > range.AmountTo)
            {
                range.AmountTo = range.AmountFrom;
            }
            
            if (range.AmountFrom.HasValue && range.AmountFrom < 0)
            {
                range.AmountFrom = 0;
            }
        }
    }

    public static void CheckIds<T>(this BaseFilterIdsRequest<T>? ids)
    {
        if (ids == null)
        {
            return;
        }

        if (ids.Ids == null)
        {
            ids.Ids = new List<T>();
        }
    }

    public static void CheckPaginator(this PaginatorEntity? paginator)
    {
        if (paginator == null)
        {
            paginator = new PaginatorEntity(1, 10, false);
        }
        
        if (paginator.PageNumber < 1)
        {
            paginator.PageNumber = 1;
        }
        
        if (paginator.PageSize < 1)
        {
            paginator.PageSize = 10;
        }
        
        if (paginator.PageSize > 100)
        {
            paginator.PageSize = 100;
        }
    }
}