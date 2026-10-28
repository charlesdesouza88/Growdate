using GrowDate.Core.Entities;

namespace GrowDate.Core.DTOs;

public static class DtoMappers
{
    public static CropDto ToDto(this Crop crop) => new CropDto
    {
        Id = crop.Id,
        Name = crop.Name,
        Slug = crop.Slug,
        Description = crop.Description,
        Category = crop.Category,
        PlantingStartMonth = crop.PlantingStartMonth,
        PlantingStartDay = crop.PlantingStartDay,
        PlantingEndMonth = crop.PlantingEndMonth,
        PlantingEndDay = crop.PlantingEndDay,
        DaysToGermination = crop.DaysToGermination,
        DaysToHarvest = crop.DaysToHarvest,
        SuitableZones = new List<string>(crop.SuitableZones),
        MinTemperature = crop.MinTemperature,
        MaxTemperature = crop.MaxTemperature,
        SoilType = crop.SoilType,
        SunRequirement = crop.SunRequirement,
        WaterRequirement = crop.WaterRequirement,
        PlantingTips = crop.PlantingTips,
        ImageUrl = string.IsNullOrWhiteSpace(crop.ImageUrl)
            ? "/images/crops/placeholder.svg"
            : crop.ImageUrl,
        Spacing = crop.Spacing,
        Companions = crop.Companions,
        Pests = crop.Pests,
        Feeding = crop.Feeding,
        HarvestOrBloomLabel = string.IsNullOrWhiteSpace(crop.HarvestOrBloomLabel)
            ? "Harvest"
            : crop.HarvestOrBloomLabel,
        CareSteps = new List<string>(crop.CareSteps)
    };

    public static RegionDto ToDto(this Region region) => new RegionDto
    {
        Id = region.Id,
        Name = region.Name,
        Country = region.Country,
        ClimateZone = region.ClimateZone,
        Latitude = region.Latitude,
        Longitude = region.Longitude
    };

    public static RecommendationDto ToDto(this PlantingRecommendation recommendation) => new RecommendationDto
    {
        Crop = recommendation.Crop.ToDto(),
        Region = recommendation.Region.ToDto(),
        SelectedDate = recommendation.SelectedDate,
        IsIdealTime = recommendation.IsIdealTime,
        PlantingWindowStart = recommendation.PlantingWindowStart,
        PlantingWindowEnd = recommendation.PlantingWindowEnd,
        EstimatedGerminationDate = recommendation.EstimatedGerminationDate,
        EstimatedHarvestDate = recommendation.EstimatedHarvestDate,
        Status = recommendation.Status,
        Notes = recommendation.Notes
    };
}
