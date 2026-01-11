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
            // Check if crop is suitable for this region
            if (!crop.SuitableZones.Any(z => z.Equals(region.ClimateZone, StringComparison.OrdinalIgnoreCase)))
                continue;

            var recommendation = CalculateRecommendation(crop, region, selectedDate);
            if (recommendation != null)
                recommendations.Add(recommendation);
        }

        return recommendations.OrderByDescending(r => r.IsIdealTime);
    }

    public async Task<IEnumerable<Crop>> GetCropsForRegionAsync(int regionId, CancellationToken cancellationToken = default)
    {
        var region = await _regionRepository.GetByIdAsync(regionId, cancellationToken);
        if (region == null)
            return Enumerable.Empty<Crop>();

        var crops = await _cropRepository.GetAllAsync(cancellationToken);
        return crops.Where(c => c.SuitableZones.Any(z => z.Equals(region.ClimateZone, StringComparison.OrdinalIgnoreCase)));
    }

    public async Task<IEnumerable<Crop>> GetCropsForDateRangeAsync(int regionId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var region = await _regionRepository.GetByIdAsync(regionId, cancellationToken);
        if (region == null)
            return Enumerable.Empty<Crop>();

        var crops = await _cropRepository.GetAllAsync(cancellationToken);
        return crops.Where(c => 
            c.SuitableZones.Any(z => z.Equals(region.ClimateZone, StringComparison.OrdinalIgnoreCase)) &&
            IsPlantingSeasonOverlap(c, startDate, endDate));
    }

    public async Task<PlantingRecommendation?> GetDetailedRecommendationAsync(int cropId, int regionId, DateTime selectedDate, CancellationToken cancellationToken = default)
    {
        var crop = await _cropRepository.GetByIdAsync(cropId, cancellationToken);
        var region = await _regionRepository.GetByIdAsync(regionId, cancellationToken);

        if (crop == null || region == null)
            return null;

        return CalculateRecommendation(crop, region, selectedDate);
    }

    private PlantingRecommendation CalculateRecommendation(Crop crop, Region region, DateTime selectedDate)
    {
        var (windowStart, windowEnd) = GetPlantingWindow(crop, selectedDate);

        var isInSeason = selectedDate >= windowStart && selectedDate <= windowEnd;
        var status = DetermineStatus(selectedDate, windowStart, windowEnd, crop);
        var harvestDate = selectedDate.AddDays(crop.DaysToGermination + crop.DaysToHarvest);

        return new PlantingRecommendation
        {
            Crop = crop,
            Region = region,
            SelectedDate = selectedDate,
            IsIdealTime = isInSeason,
            PlantingWindowStart = windowStart,
            PlantingWindowEnd = windowEnd,
            EstimatedGerminationDate = selectedDate.AddDays(crop.DaysToGermination),
            EstimatedHarvestDate = harvestDate,
            Status = status,
            Notes = GenerateNotes(crop, region, isInSeason, status)
        };
    }

    private string DetermineStatus(DateTime selectedDate, DateTime plantingStart, DateTime plantingEnd, Crop crop)
    {
        if (selectedDate >= plantingStart && selectedDate <= plantingEnd)
            return "Ideal";

        var daysUntilStart = (plantingStart - selectedDate).Days;
        var daysPastEnd = (selectedDate - plantingEnd).Days;

        if (daysUntilStart > 0 && daysUntilStart <= 30)
            return "Coming Soon";
        if (daysPastEnd > 0 && daysPastEnd <= 30)
            return "Late Season";

        return "Out of Season";
    }

    private string GenerateNotes(Crop crop, Region region, bool isInSeason, string status)
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

        notes.Add($"Climate Zone: {region.ClimateZone}");
        notes.Add($"Germination: {crop.DaysToGermination} days");
        notes.Add($"Harvest: {crop.DaysToHarvest} days after germination");

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

    private bool IsPlantingSeasonOverlap(Crop crop, DateTime startDate, DateTime endDate)
    {
        var (windowStart, windowEnd) = GetPlantingWindow(crop, startDate);
        return windowStart <= endDate && windowEnd >= startDate;
    }

    private (DateTime Start, DateTime End) GetPlantingWindow(Crop crop, DateTime selectedDate)
    {
        var year = selectedDate.Year;
        var start = new DateTime(year, crop.PlantingStartMonth, crop.PlantingStartDay);
        var end = new DateTime(year, crop.PlantingEndMonth, crop.PlantingEndDay);

        // Handle wrap-around seasons (e.g., Nov-Feb)
        if (start <= end)
            return (start, end);

        // If selected date is in the early-year segment (before or on end), season started last year
        if (selectedDate <= end)
        {
            start = start.AddYears(-1);
            return (start, end);
        }

        // Otherwise season continues into next year
        end = end.AddYears(1);
        return (start, end);
    }
}
