using CommonModule.Shared.Requests.Base;

namespace CommonModule.Core.Extensions;

public static class FilterExtension
{
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
}