using GrowDate.Core.Entities;

namespace GrowDate.Core.Interfaces;

/// <summary>
/// Repository interface for Crop data access
/// </summary>
public interface ICropRepository
{
    Task<IEnumerable<Crop>> GetAllAsync();
    Task<Crop?> GetByIdAsync(int id);
    Task<IEnumerable<Crop>> GetByCategoryAsync(string category);
    Task<IEnumerable<Crop>> GetBySuitableZoneAsync(string zone);
    Task<Crop> AddAsync(Crop crop);
    Task UpdateAsync(Crop crop);
    Task DeleteAsync(int id);
}
