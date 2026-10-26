using GrowDate.Core.Services;
using Xunit;

namespace GrowDate.Tests.Services;

public class PlantingWindowCalculatorTests
{
    [Fact]
    public void GetWindow_NorthernHemisphere_KeepsAuthoredMonths()
    {
        var (start, end) = PlantingWindowCalculator.GetWindow(3, 15, 6, 30, latitude: 36.7, new DateTime(2024, 4, 1));

        Assert.Equal(new DateTime(2024, 3, 15), start);
        Assert.Equal(new DateTime(2024, 6, 30), end);
    }

    [Fact]
    public void GetWindow_SouthernHemisphere_ShiftsSixMonths()
    {
        var (start, end) = PlantingWindowCalculator.GetWindow(3, 15, 6, 30, latitude: -23.55, new DateTime(2024, 10, 1));

        Assert.Equal(new DateTime(2024, 9, 15), start);
        Assert.Equal(new DateTime(2024, 12, 30), end);
    }

    [Fact]
    public void DetermineStatus_ComingSoon_WithinThirtyDaysOfStart()
    {
        var status = PlantingWindowCalculator.DetermineStatus(
            new DateTime(2024, 2, 20),
            new DateTime(2024, 3, 15),
            new DateTime(2024, 6, 30));

        Assert.Equal("Coming Soon", status);
    }

    [Fact]
    public void DetermineStatus_LateSeason_AfterWrappedWinterWindow()
    {
        var status = PlantingWindowCalculator.DetermineStatus(
            new DateTime(2024, 3, 1),
            new DateTime(2024, 11, 1),
            new DateTime(2025, 2, 15));

        Assert.Equal("Late Season", status);
    }

    [Fact]
    public void OverlapsRange_DetectsSeasonInNextCalendarYear()
    {
        var overlaps = PlantingWindowCalculator.OverlapsRange(
            3, 15, 6, 30, latitude: 36.7,
            rangeStart: new DateTime(2024, 12, 1),
            rangeEnd: new DateTime(2025, 3, 20));

        Assert.True(overlaps);
    }

    [Fact]
    public void SafeDate_ClampsInvalidDay()
    {
        var date = PlantingWindowCalculator.SafeDate(2023, 2, 31);

        Assert.Equal(new DateTime(2023, 2, 28), date);
    }
}
