using GrowDate.Core.Entities;
using GrowDate.Core.Interfaces;
using GrowDate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowDate.Infrastructure.Repositories;

public class CropRepository : ICropRepository
{
    private readonly GrowDateDbContext _context;

    public CropRepository(GrowDateDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Crop>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Crops
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Crop?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Crops.FindAsync(new object?[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Crop>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _context.Crops
            .AsNoTracking()
            .Where(c => c.Category == category)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Crop>> GetBySuitableZoneAsync(string zone, CancellationToken cancellationToken = default)
    {
        var normalizedZone = zone?.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(normalizedZone))
            return Enumerable.Empty<Crop>();

        // Match whole tokens to avoid substring collisions (e.g., "Zone 1" vs "Zone 10")
        var pattern = $"%,{normalizedZone},%";

        return await _context.Crops
            .AsNoTracking()
            .Where(c => EF.Functions.Like("," + c.SuitableZonesCsv.ToLower() + ",", pattern))
            .ToListAsync(cancellationToken);
    }

    public async Task<Crop> AddAsync(Crop crop, CancellationToken cancellationToken = default)
    {
        _context.Crops.Add(crop);
        await _context.SaveChangesAsync(cancellationToken);
        return crop;
    }

    public async Task UpdateAsync(Crop crop, CancellationToken cancellationToken = default)
    {
        _context.Entry(crop).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var crop = await _context.Crops.FindAsync(new object?[] { id }, cancellationToken);
        if (crop != null)
        {
            _context.Crops.Remove(crop);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
