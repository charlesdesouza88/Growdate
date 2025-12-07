using GrowDate.Core.Entities;

namespace GrowDate.Core.Interfaces;

/// <summary>
/// Service interface for generating planting recommendations
/// </summary>
public interface IRecommendationService
{
    Task<IEnumerable<PlantingRecommendation>> GetRecommendationsAsync(int regionId, DateTime selectedDate);
    Task<IEnumerable<Crop>> GetCropsForRegionAsync(int regionId);
    Task<IEnumerable<Crop>> GetCropsForDateRangeAsync(int regionId, DateTime startDate, DateTime endDate);
    Task<PlantingRecommendation?> GetDetailedRecommendationAsync(int cropId, int regionId, DateTime selectedDate);
}
