namespace GrowDate.Core.Services;

/// <summary>
/// Shared planting-window math used by the recommendation engine and the calendar UI.
/// Crop seed data is authored for the Northern Hemisphere; Southern Hemisphere
/// regions (latitude &lt; 0) are shifted by six months as a first approximation.
/// </summary>
public static class PlantingWindowCalculator
{
    public static (DateTime Start, DateTime End) GetWindow(
        int startMonth,
        int startDay,
        int endMonth,
        int endDay,
        double latitude,
        DateTime selectedDate)
    {
        if (latitude < 0)
        {
            (startMonth, startDay) = ShiftMonths(startMonth, startDay, 6, selectedDate.Year);
            (endMonth, endDay) = ShiftMonths(endMonth, endDay, 6, selectedDate.Year);
        }

        var year = selectedDate.Year;
        var start = SafeDate(year, startMonth, startDay);
        var end = SafeDate(year, endMonth, endDay);

        if (start <= end)
            return (start, end);

        if (selectedDate.Date <= end)
            return (start.AddYears(-1), end);

        return (start, end.AddYears(1));
    }

    public static bool OverlapsRange(
        int startMonth,
        int startDay,
        int endMonth,
        int endDay,
        double latitude,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        rangeStart = rangeStart.Date;
        rangeEnd = rangeEnd.Date;

        for (var year = rangeStart.Year - 1; year <= rangeEnd.Year + 1; year++)
        {
            var probe = new DateTime(year, 6, 15);
            var (windowStart, windowEnd) = GetWindow(startMonth, startDay, endMonth, endDay, latitude, probe);
            if (windowStart <= rangeEnd && windowEnd >= rangeStart)
                return true;

            // Wrap-around windows differ for early-year vs late-year probes.
            probe = new DateTime(year, 1, 15);
            (windowStart, windowEnd) = GetWindow(startMonth, startDay, endMonth, endDay, latitude, probe);
            if (windowStart <= rangeEnd && windowEnd >= rangeStart)
                return true;
        }

        return false;
    }

    public static string DetermineStatus(DateTime selectedDate, DateTime plantingStart, DateTime plantingEnd)
    {
        selectedDate = selectedDate.Date;
        plantingStart = plantingStart.Date;
        plantingEnd = plantingEnd.Date;

        if (selectedDate >= plantingStart && selectedDate <= plantingEnd)
            return "Ideal";

        var daysUntilStart = (plantingStart - selectedDate).Days;
        if (daysUntilStart > 0 && daysUntilStart <= 30)
            return "Coming Soon";

        var daysUntilNext = (plantingStart.AddYears(1) - selectedDate).Days;
        if (daysUntilNext > 0 && daysUntilNext <= 30)
            return "Coming Soon";

        var daysPastEnd = (selectedDate - plantingEnd).Days;
        if (daysPastEnd > 0 && daysPastEnd <= 30)
            return "Late Season";

        if (selectedDate < plantingStart)
        {
            var daysPastPreviousEnd = (selectedDate - plantingEnd.AddYears(-1)).Days;
            if (daysPastPreviousEnd > 0 && daysPastPreviousEnd <= 30)
                return "Late Season";
        }

        return "Out of Season";
    }

    internal static (int Month, int Day) ShiftMonths(int month, int day, int delta, int yearHint)
    {
        var monthIndex = month - 1 + delta;
        var year = yearHint + Math.DivRem(monthIndex, 12, out var remainder);
        if (remainder < 0)
        {
            remainder += 12;
            year -= 1;
        }

        var shiftedMonth = remainder + 1;
        var dim = DateTime.DaysInMonth(year, shiftedMonth);
        return (shiftedMonth, Math.Min(day, dim));
    }

    public static DateTime SafeDate(int year, int month, int day)
    {
        var dim = DateTime.DaysInMonth(year, month);
        return new DateTime(year, month, Math.Min(Math.Max(day, 1), dim));
    }
}
