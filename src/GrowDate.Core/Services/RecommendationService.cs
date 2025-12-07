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

    public async Task<IEnumerable<PlantingRecommendation>> GetRecommendationsAsync(int regionId, DateTime selectedDate)
    {
        var region = await _regionRepository.GetByIdAsync(regionId);
        if (region == null)
            return Enumerable.Empty<PlantingRecommendation>();

        var crops = await _cropRepository.GetAllAsync();
        var recommendations = new List<PlantingRecommendation>();

        foreach (var crop in crops)
        {
            // Check if crop is suitable for this region
            if (!crop.SuitableZones.Contains(region.ClimateZone))
                continue;

            var recommendation = CalculateRecommendation(crop, region, selectedDate);
            if (recommendation != null)
                recommendations.Add(recommendation);
        }

        return recommendations.OrderByDescending(r => r.IsIdealTime);
    }

    public async Task<IEnumerable<Crop>> GetCropsForRegionAsync(int regionId)
    {
        var region = await _regionRepository.GetByIdAsync(regionId);
        if (region == null)
            return Enumerable.Empty<Crop>();

        var crops = await _cropRepository.GetAllAsync();
        return crops.Where(c => c.SuitableZones.Contains(region.ClimateZone));
    }

    public async Task<IEnumerable<Crop>> GetCropsForDateRangeAsync(int regionId, DateTime startDate, DateTime endDate)
    {
        var region = await _regionRepository.GetByIdAsync(regionId);
        if (region == null)
            return Enumerable.Empty<Crop>();

        var crops = await _cropRepository.GetAllAsync();
        return crops.Where(c => 
            c.SuitableZones.Contains(region.ClimateZone) &&
            IsPlantingSeasonOverlap(c, startDate, endDate));
    }

    public async Task<PlantingRecommendation> GetDetailedRecommendationAsync(int cropId, int regionId, DateTime selectedDate)
    {
        var crop = await _cropRepository.GetByIdAsync(cropId);
        var region = await _regionRepository.GetByIdAsync(regionId);

        if (crop == null || region == null)
            return null;

        return CalculateRecommendation(crop, region, selectedDate);
    }

    private PlantingRecommendation CalculateRecommendation(Crop crop, Region region, DateTime selectedDate)
    {
        // Extract month and day for comparison (ignoring year)
        var selectedMonthDay = new DateTime(2000, selectedDate.Month, selectedDate.Day);
        var plantingStart = new DateTime(2000, crop.PlantingStartMonth, crop.PlantingStartDay);
        var plantingEnd = new DateTime(2000, crop.PlantingEndMonth, crop.PlantingEndDay);

        // Handle wrap-around seasons (e.g., Nov-Feb)
        bool isInSeason;
        if (plantingStart <= plantingEnd)
        {
            isInSeason = selectedMonthDay >= plantingStart && selectedMonthDay <= plantingEnd;
        }
        else
        {
            isInSeason = selectedMonthDay >= plantingStart || selectedMonthDay <= plantingEnd;
        }

        var status = DetermineStatus(selectedMonthDay, plantingStart, plantingEnd, crop);
        var harvestDate = selectedDate.AddDays(crop.DaysToGermination + crop.DaysToHarvest);

        return new PlantingRecommendation
        {
            Crop = crop,
            Region = region,
            SelectedDate = selectedDate,
            IsIdealTime = isInSeason,
            PlantingWindowStart = new DateTime(selectedDate.Year, crop.PlantingStartMonth, crop.PlantingStartDay),
            PlantingWindowEnd = new DateTime(selectedDate.Year, crop.PlantingEndMonth, crop.PlantingEndDay),
            EstimatedGerminationDate = selectedDate.AddDays(crop.DaysToGermination),
            EstimatedHarvestDate = harvestDate,
            Status = status,
            Notes = GenerateNotes(crop, region, isInSeason, status)
        };
    }

    private string DetermineStatus(DateTime selectedDate, DateTime plantingStart, DateTime plantingEnd, Crop crop)
    {
        if (plantingStart <= plantingEnd)
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
        else
        {
            // Wrap-around season
            if (selectedDate >= plantingStart || selectedDate <= plantingEnd)
                return "Ideal";
            
            return "Out of Season";
        }
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
        var cropStart = new DateTime(startDate.Year, crop.PlantingStartMonth, crop.PlantingStartDay);
        var cropEnd = new DateTime(startDate.Year, crop.PlantingEndMonth, crop.PlantingEndDay);

        // Handle wrap-around
        if (cropStart > cropEnd)
        {
            cropEnd = cropEnd.AddYears(1);
        }

        return cropStart <= endDate && cropEnd >= startDate;
    }
}
