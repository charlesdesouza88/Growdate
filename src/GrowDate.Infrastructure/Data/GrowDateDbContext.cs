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
    public DbSet<SeedMeta> SeedMeta { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Crop>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Slug).HasMaxLength(200);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.HarvestOrBloomLabel).HasMaxLength(40);
            entity.Property(e => e.ImageUrl).HasMaxLength(300);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.PlantingTips).HasMaxLength(2000);
            entity.Property(e => e.Feeding).HasMaxLength(1000);
            entity.Property(e => e.Pests).HasMaxLength(500);
            entity.Property(e => e.Companions).HasMaxLength(500);
            entity.Property(e => e.Spacing).HasMaxLength(200);
            entity.Property(e => e.CareStepsText).HasMaxLength(2000);

            entity.Property(e => e.SuitableZonesCsv)
                .IsRequired()
                .HasColumnName("SuitableZones")
                .HasMaxLength(500);
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.ClimateZone).HasMaxLength(50);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<SeedMeta>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}
