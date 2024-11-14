using CommonModule.Shared.Enums.Expenses;

namespace CommonModule.Core.Extensions;

public static class DateTimeExtension
{
    public static DateTime GetStartOfCurrentMonth()
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToUniversalTime();
    }

    public static DateTime GetEndOfCurrentMonth()
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month,
            DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 23, 59, 59).ToUniversalTime();
    }

    public static DateTime GetStartOfCurrentDay()
    {
        return DateTime.Today.ToUniversalTime();
    }

    public static DateTime GetEndOfCurrentDay()
    {
        return DateTime.Today.AddDays(1).AddTicks(-1).ToUniversalTime();
    }

    public static DateTime GetStartOfCurrentHour()
    {
        var now = DateTime.Now;
        return new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).ToUniversalTime();
    }

    public static DateTime GetEndOfCurrentHour()
    {
        var now = DateTime.Now;
        return new DateTime(now.Year, now.Month, now.Day, now.Hour, 59, 59).ToUniversalTime();
    }

    public static DateTime GetStartOfCurrentYear()
    {
        return new DateTime(DateTime.Now.Year, 1, 1).ToUniversalTime();
    }

    public static DateTime GetEndOfCurrentYear()
    {
        return new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59).ToUniversalTime();
    }

    public static void SetMidnight(this DateTime? date)
    {
        if (date.HasValue)
        {
            date = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0).ToUniversalTime();
        }
    }

    public static DateTime GetNextDate(this DateTime startDate, int frequencyId)
    {
        FrequencyEnum frequency = (FrequencyEnum)frequencyId;
        DateTime nextDate = startDate;

        nextDate = AddFrequency(nextDate, frequency).ToUniversalTime();;

        while (nextDate <= DateTime.UtcNow)
        {
            nextDate = AddFrequency(nextDate, frequency).ToUniversalTime();;
        }

        return nextDate;
    }

    private static DateTime AddFrequency(DateTime date, FrequencyEnum frequency)
    {
        switch (frequency)
        {
            case FrequencyEnum.Daily:
                return date.AddDays(1);
            case FrequencyEnum.Weekly:
                return date.AddDays(7);
            case FrequencyEnum.BiWeekly:
                return date.AddDays(14);
            case FrequencyEnum.Monthly:
                return date.AddMonths(1);
            case FrequencyEnum.Quarterly:
                return date.AddMonths(3);
            case FrequencyEnum.SemiAnnual:
                return date.AddMonths(6);
            case FrequencyEnum.Annual:
                return date.AddYears(1);
            default:
                return date;
        }
    }
}