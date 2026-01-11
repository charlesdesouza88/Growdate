using GrowDate.Core.Entities;

namespace GrowDate.Core.Interfaces;

/// <summary>
/// Repository interface for Crop data access
/// </summary>
public interface ICropRepository
{
    Task<IEnumerable<Crop>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Crop?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Crop>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<IEnumerable<Crop>> GetBySuitableZoneAsync(string zone, CancellationToken cancellationToken = default);
    Task<Crop> AddAsync(Crop crop, CancellationToken cancellationToken = default);
    Task UpdateAsync(Crop crop, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
