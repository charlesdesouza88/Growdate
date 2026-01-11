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

    public async Task<IEnumerable<Region>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Regions.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Region?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Regions.FindAsync(new object?[] { id }, cancellationToken);
    }

    public async Task<Region?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.Regions
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<Region>> GetByCountryAsync(string country, CancellationToken cancellationToken = default)
    {
        return await _context.Regions
            .AsNoTracking()
            .Where(r => r.Country == country)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Region>> GetByClimateZoneAsync(string zone, CancellationToken cancellationToken = default)
    {
        return await _context.Regions
            .AsNoTracking()
            .Where(r => r.ClimateZone == zone)
            .ToListAsync(cancellationToken);
    }

    public async Task<Region> AddAsync(Region region, CancellationToken cancellationToken = default)
    {
        _context.Regions.Add(region);
        await _context.SaveChangesAsync(cancellationToken);
        return region;
    }

    public async Task UpdateAsync(Region region, CancellationToken cancellationToken = default)
    {
        _context.Entry(region).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var region = await _context.Regions.FindAsync(new object?[] { id }, cancellationToken);
        if (region != null)
        {
            _context.Regions.Remove(region);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
