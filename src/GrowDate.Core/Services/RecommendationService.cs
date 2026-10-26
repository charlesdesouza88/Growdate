using GrowDate.Core.Entities;
using GrowDate.Core.Interfaces;

namespace GrowDate.Core.Services;

public class RecommendationService : IRecommendationService
{
    private readonly ICropRepository _cropRepository;
    private readonly IRegionRepository _regionRepository;

    public RecommendationService(ICropRepository cropRepository, IRegionRepository regionRepository)
    {
        _cropRepository = cropRepository;
        _regionRepository = regionRepository;
    }

    public async Task<IEnumerable<PlantingRecommendation>> GetRecommendationsAsync(int regionId, DateTime selectedDate, CancellationToken cancellationToken = default)
    {
        var region = await _regionRepository.GetByIdAsync(regionId, cancellationToken);
        if (region == null)
            return Enumerable.Empty<PlantingRecommendation>();

        var crops = await _cropRepository.GetAllAsync(cancellationToken);
        var recommendations = new List<PlantingRecommendation>();

        foreach (var crop in crops)
        {
            if (!IsSuitableForRegion(crop, region))
                continue;

            recommendations.Add(CalculateRecommendation(crop, region, selectedDate));
        }

        return recommendations.OrderByDescending(r => r.IsIdealTime);
    }

    public async Task<IEnumerable<Crop>> GetCropsForRegionAsync(int regionId, CancellationToken cancellationToken = default)
    {
        var region = await _regionRepository.GetByIdAsync(regionId, cancellationToken);
        if (region == null)
            return Enumerable.Empty<Crop>();

        var crops = await _cropRepository.GetAllAsync(cancellationToken);
        return crops.Where(c => IsSuitableForRegion(c, region));
    }

    public async Task<IEnumerable<Crop>> GetCropsForDateRangeAsync(int regionId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var region = await _regionRepository.GetByIdAsync(regionId, cancellationToken);
        if (region == null)
            return Enumerable.Empty<Crop>();

        var crops = await _cropRepository.GetAllAsync(cancellationToken);
        return crops.Where(c =>
            IsSuitableForRegion(c, region) &&
            PlantingWindowCalculator.OverlapsRange(
                c.PlantingStartMonth, c.PlantingStartDay,
                c.PlantingEndMonth, c.PlantingEndDay,
                region.Latitude, startDate, endDate));
    }

    public async Task<PlantingRecommendation?> GetDetailedRecommendationAsync(int cropId, int regionId, DateTime selectedDate, CancellationToken cancellationToken = default)
    {
        var crop = await _cropRepository.GetByIdAsync(cropId, cancellationToken);
        var region = await _regionRepository.GetByIdAsync(regionId, cancellationToken);

        if (crop == null || region == null)
            return null;

        return CalculateRecommendation(crop, region, selectedDate);
    }

    private static bool IsSuitableForRegion(Crop crop, Region region) =>
        crop.SuitableZones.Any(z => z.Equals(region.ClimateZone, StringComparison.OrdinalIgnoreCase));

    private static PlantingRecommendation CalculateRecommendation(Crop crop, Region region, DateTime selectedDate)
    {
        selectedDate = selectedDate.Date;
        var (windowStart, windowEnd) = PlantingWindowCalculator.GetWindow(
            crop.PlantingStartMonth, crop.PlantingStartDay,
            crop.PlantingEndMonth, crop.PlantingEndDay,
            region.Latitude, selectedDate);

        var isInSeason = selectedDate >= windowStart && selectedDate <= windowEnd;
        var status = PlantingWindowCalculator.DetermineStatus(selectedDate, windowStart, windowEnd);

        return new PlantingRecommendation
        {
            Crop = crop,
            Region = region,
            SelectedDate = selectedDate,
            IsIdealTime = isInSeason,
            PlantingWindowStart = windowStart,
            PlantingWindowEnd = windowEnd,
            EstimatedGerminationDate = selectedDate.AddDays(crop.DaysToGermination),
            EstimatedHarvestDate = selectedDate.AddDays(crop.DaysToHarvest),
            Status = status,
            Notes = GenerateNotes(crop, region, isInSeason, status)
        };
    }

    private static string GenerateNotes(Crop crop, Region region, bool isInSeason, string status)
    {
        var notes = new List<string>();

        if (isInSeason)
        {
            notes.Add($"Perfect time to plant {crop.Name} in {region.Name}.");
        }
        else
        {
            notes.Add($"{crop.Name} is currently out of season in {region.Name}.");
        }

        if (region.Latitude < 0)
            notes.Add("Planting window adjusted for the Southern Hemisphere.");

        notes.Add($"Climate Zone: {region.ClimateZone}");
        notes.Add($"Germination: {crop.DaysToGermination} days");
        notes.Add($"Harvest: {crop.DaysToHarvest} days after planting");

        if (status == "Coming Soon")
        {
            notes.Add("Planting season begins soon - prepare your soil!");
        }
        else if (status == "Late Season")
        {
            notes.Add("Late in the season - consider starting indoors or waiting for next cycle.");
        }

        return string.Join(" ", notes);
    }
}
