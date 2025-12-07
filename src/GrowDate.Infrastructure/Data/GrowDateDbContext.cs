using GrowDate.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrowDate.Infrastructure.Data;

public class GrowDateDbContext : DbContext
{
    public GrowDateDbContext(DbContextOptions<GrowDateDbContext> options)
        : base(options)
    {
    }

    public DbSet<Crop> Crops { get; set; } = null!;
    public DbSet<Region> Regions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Crop entity
        modelBuilder.Entity<Crop>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Category).HasMaxLength(50);
            
            // Store SuitableZones as JSON
            entity.Property(e => e.SuitableZones)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                );
        });

        // Configure Region entity
        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.ClimateZone).HasMaxLength(50);
            entity.HasIndex(e => e.Code).IsUnique();
        });
    }
}
