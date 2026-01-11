using GrowDate.Core.Entities;

namespace GrowDate.Core.Interfaces;

/// <summary>
/// Service interface for generating planting recommendations
/// </summary>
public interface IRecommendationService
{
    Task<IEnumerable<PlantingRecommendation>> GetRecommendationsAsync(int regionId, DateTime selectedDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Crop>> GetCropsForRegionAsync(int regionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Crop>> GetCropsForDateRangeAsync(int regionId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<PlantingRecommendation?> GetDetailedRecommendationAsync(int cropId, int regionId, DateTime selectedDate, CancellationToken cancellationToken = default);
}
