using GrowDate.Core.Entities;

namespace GrowDate.Core.Interfaces;

/// <summary>
/// Repository interface for Region data access
/// </summary>
public interface IRegionRepository
{
    Task<IEnumerable<Region>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Region?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Region?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Region>> GetByCountryAsync(string country, CancellationToken cancellationToken = default);
    Task<IEnumerable<Region>> GetByClimateZoneAsync(string zone, CancellationToken cancellationToken = default);
    Task<Region> AddAsync(Region region, CancellationToken cancellationToken = default);
    Task UpdateAsync(Region region, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
