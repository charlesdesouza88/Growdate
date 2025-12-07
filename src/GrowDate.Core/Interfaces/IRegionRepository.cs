using GrowDate.Core.Entities;

namespace GrowDate.Core.Interfaces;

/// <summary>
/// Repository interface for Region data access
/// </summary>
public interface IRegionRepository
{
    Task<IEnumerable<Region>> GetAllAsync();
    Task<Region?> GetByIdAsync(int id);
    Task<Region?> GetByCodeAsync(string code);
    Task<IEnumerable<Region>> GetByCountryAsync(string country);
    Task<IEnumerable<Region>> GetByClimateZoneAsync(string zone);
    Task<Region> AddAsync(Region region);
    Task UpdateAsync(Region region);
    Task DeleteAsync(int id);
}
