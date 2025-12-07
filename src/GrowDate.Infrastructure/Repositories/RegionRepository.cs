using GrowDate.Core.Entities;
using GrowDate.Core.Interfaces;
using GrowDate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowDate.Infrastructure.Repositories;

public class RegionRepository : IRegionRepository
{
    private readonly GrowDateDbContext _context;

    public RegionRepository(GrowDateDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Region>> GetAllAsync()
    {
        return await _context.Regions.ToListAsync();
    }

    public async Task<Region?> GetByIdAsync(int id)
    {
        return await _context.Regions.FindAsync(id);
    }

    public async Task<Region?> GetByCodeAsync(string code)
    {
        return await _context.Regions
            .FirstOrDefaultAsync(r => r.Code == code);
    }

    public async Task<IEnumerable<Region>> GetByCountryAsync(string country)
    {
        return await _context.Regions
            .Where(r => r.Country == country)
            .ToListAsync();
    }

    public async Task<IEnumerable<Region>> GetByClimateZoneAsync(string zone)
    {
        return await _context.Regions
            .Where(r => r.ClimateZone == zone)
            .ToListAsync();
    }

    public async Task<Region> AddAsync(Region region)
    {
        _context.Regions.Add(region);
        await _context.SaveChangesAsync();
        return region;
    }

    public async Task UpdateAsync(Region region)
    {
        _context.Entry(region).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var region = await _context.Regions.FindAsync(id);
        if (region != null)
        {
            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
        }
    }
}
