using GrowDate.Core.Entities;

namespace GrowDate.Core.DTOs;

public static class DtoMappers
{
    public static CropDto ToDto(this Crop crop) => new CropDto
    {
        Id = crop.Id,
        Name = crop.Name,
        Category = crop.Category,
        PlantingStartMonth = crop.PlantingStartMonth,
        PlantingStartDay = crop.PlantingStartDay,
        PlantingEndMonth = crop.PlantingEndMonth,
        PlantingEndDay = crop.PlantingEndDay,
        DaysToGermination = crop.DaysToGermination,
        DaysToHarvest = crop.DaysToHarvest,
        SuitableZones = new List<string>(crop.SuitableZones)
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
