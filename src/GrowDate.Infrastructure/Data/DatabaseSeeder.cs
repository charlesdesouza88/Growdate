using System.Reflection;
using System.Text.Json;
using GrowDate.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrowDate.Infrastructure.Data;

public static class DatabaseSeeder
{
    public const int CurrentSeedVersion = 2;
    private const string CatalogResource = "GrowDate.Infrastructure.Data.catalog.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static CatalogDocument LoadCatalog()
    {
        var assembly = typeof(DatabaseSeeder).Assembly;
        using var stream = assembly.GetManifestResourceStream(CatalogResource)
            ?? throw new InvalidOperationException($"Embedded catalog not found: {CatalogResource}. Resources: {string.Join(", ", assembly.GetManifestResourceNames())}");
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<CatalogDocument>(json, JsonOptions)
            ?? throw new InvalidOperationException("Catalog JSON was empty.");
    }

    public static async Task SeedAsync(GrowDateDbContext context)
    {
        if (!context.Regions.Any())
            await SeedRegionsAsync(context);

        var catalog = LoadCatalog();
        var existing = await context.Crops.ToListAsync();
        var byName = existing.ToDictionary(c => c.Name, StringComparer.OrdinalIgnoreCase);
        var now = DateTime.UtcNow;

        foreach (var entry in catalog.Crops)
        {
            if (byName.TryGetValue(entry.Name, out var crop))
            {
                Apply(crop, entry, now, created: crop.CreatedAt);
            }
            else
            {
                var created = ToCrop(entry, now);
                context.Crops.Add(created);
                byName[entry.Name] = created;
            }
        }

        var meta = context.SeedMeta.FirstOrDefault();
        if (meta == null)
            context.SeedMeta.Add(new SeedMeta { Version = CurrentSeedVersion });
        else
            meta.Version = CurrentSeedVersion;

        await context.SaveChangesAsync();
    }

    public static Crop ToCrop(CatalogCrop entry, DateTime now)
    {
        var crop = new Crop { CreatedAt = now };
        Apply(crop, entry, now, now);
        return crop;
    }

    private static void Apply(Crop crop, CatalogCrop entry, DateTime now, DateTime created)
    {
        crop.Name = entry.Name;
        crop.Slug = string.IsNullOrWhiteSpace(entry.Slug) ? Slugify(entry.Name) : entry.Slug;
        crop.Category = entry.Category;
        crop.Description = entry.Description;
        crop.PlantingStartMonth = entry.PlantingStartMonth;
        crop.PlantingStartDay = entry.PlantingStartDay;
        crop.PlantingEndMonth = entry.PlantingEndMonth;
        crop.PlantingEndDay = entry.PlantingEndDay;
        crop.DaysToGermination = entry.DaysToGermination;
        crop.DaysToHarvest = entry.DaysToHarvest;
        crop.SuitableZones = entry.SuitableZones;
        crop.MinTemperature = entry.MinTemperature;
        crop.MaxTemperature = entry.MaxTemperature;
        crop.SoilType = entry.SoilType;
        crop.SunRequirement = entry.SunRequirement;
        crop.WaterRequirement = entry.WaterRequirement;
        crop.PlantingTips = entry.PlantingTips;
        crop.Spacing = entry.Spacing;
        crop.Companions = entry.Companions;
        crop.Pests = entry.Pests;
        crop.Feeding = entry.Feeding;
        crop.CareSteps = entry.CareSteps;
        crop.HarvestOrBloomLabel = string.IsNullOrWhiteSpace(entry.HarvestOrBloomLabel) ? "Harvest" : entry.HarvestOrBloomLabel;
        crop.ImageUrl = string.IsNullOrWhiteSpace(entry.ImageUrl)
            ? $"/images/crops/{crop.Slug}.svg"
            : entry.ImageUrl;
        crop.CreatedAt = created;
        crop.UpdatedAt = now;
    }

    private static string Slugify(string name) =>
        name.Trim().ToLowerInvariant().Replace(' ', '-');

    private static async Task SeedRegionsAsync(GrowDateDbContext context)
    {
        var now = DateTime.UtcNow;
        var regions = new List<Region>
        {
            new()
            {
                Name = "California Central Valley",
                Code = "US_CA_CENTRAL",
                Country = "USA",
                ClimateZone = "Zone 9",
                ClimateType = "Mediterranean",
                Description = "Warm Mediterranean climate ideal for year-round growing",
                Latitude = 36.7783,
                Longitude = -119.4179,
                AverageMinTemp = 5,
                AverageMaxTemp = 35,
                FrostFreeDays = 300,
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Name = "Florida",
                Code = "US_FL",
                Country = "USA",
                ClimateZone = "Zone 10",
                ClimateType = "Subtropical",
                Description = "Humid subtropical climate with mild winters",
                Latitude = 27.9944,
                Longitude = -81.7603,
                AverageMinTemp = 15,
                AverageMaxTemp = 33,
                FrostFreeDays = 365,
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Name = "Texas Hill Country",
                Code = "US_TX_HILL",
                Country = "USA",
                ClimateZone = "Zone 8",
                ClimateType = "Temperate",
                Description = "Variable climate with hot summers and mild winters",
                Latitude = 30.2672,
                Longitude = -98.7453,
                AverageMinTemp = 0,
                AverageMaxTemp = 36,
                FrostFreeDays = 250,
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Name = "São Paulo State",
                Code = "BR_SP",
                Country = "Brazil",
                ClimateZone = "Tropical",
                ClimateType = "Tropical",
                Description = "Tropical climate with distinct wet and dry seasons",
                Latitude = -23.5505,
                Longitude = -46.6333,
                AverageMinTemp = 12,
                AverageMaxTemp = 28,
                FrostFreeDays = 365,
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Name = "Minas Gerais",
                Code = "BR_MG",
                Country = "Brazil",
                ClimateZone = "Subtropical",
                ClimateType = "Subtropical",
                Description = "Subtropical highland climate with cooler temperatures",
                Latitude = -19.9167,
                Longitude = -43.9345,
                AverageMinTemp = 10,
                AverageMaxTemp = 26,
                FrostFreeDays = 330,
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Name = "Pacific Northwest",
                Code = "US_PNW",
                Country = "USA",
                ClimateZone = "Zone 8",
                ClimateType = "Oceanic",
                Description = "Cool, wet winters and warm, dry summers",
                Latitude = 47.6062,
                Longitude = -122.3321,
                AverageMinTemp = 2,
                AverageMaxTemp = 26,
                FrostFreeDays = 220,
                CreatedAt = now,
                UpdatedAt = now
            }
        };

        context.Regions.AddRange(regions);
        await context.SaveChangesAsync();
    }
}
