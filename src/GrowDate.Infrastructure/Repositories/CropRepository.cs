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

    public async Task<IEnumerable<Crop>> GetAllAsync()
    {
        return await _context.Crops.ToListAsync();
    }

    public async Task<Crop?> GetByIdAsync(int id)
    {
        return await _context.Crops.FindAsync(id);
    }

    public async Task<IEnumerable<Crop>> GetByCategoryAsync(string category)
    {
        return await _context.Crops
            .Where(c => c.Category == category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Crop>> GetBySuitableZoneAsync(string zone)
    {
        // Load all crops into memory and filter in-memory since SuitableZones is stored as comma-separated string
        var allCrops = await _context.Crops.ToListAsync();
        return allCrops.Where(c => c.SuitableZones.Contains(zone)).ToList();
    }

    public async Task<Crop> AddAsync(Crop crop)
    {
        _context.Crops.Add(crop);
        await _context.SaveChangesAsync();
        return crop;
    }

    public async Task UpdateAsync(Crop crop)
    {
        _context.Entry(crop).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var crop = await _context.Crops.FindAsync(id);
        if (crop != null)
        {
            _context.Crops.Remove(crop);
            await _context.SaveChangesAsync();
        }
    }
}
