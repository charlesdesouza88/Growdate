using GrowDate.Core.Entities;

namespace GrowDate.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(GrowDateDbContext context)
    {
        // Check if already seeded
        if (context.Regions.Any() || context.Crops.Any())
            return;

        // Seed Regions
        var regions = new List<Region>
        {
            new Region
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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Region
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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Region
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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Region
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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Region
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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Region
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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Regions.AddRange(regions);
        await context.SaveChangesAsync();

        // Seed Crops with default values
        var now = DateTime.UtcNow;
        var crops = new List<Crop>
        {
            CreateCrop("Tomatoes", "Vegetable", 3, 15, 6, 30, 7, 70, 
                new[] { "Zone 8", "Zone 9", "Zone 10", "Tropical", "Subtropical" }, now),
            CreateCrop("Lettuce", "Vegetable", 2, 1, 5, 31, 5, 45,
                new[] { "Zone 8", "Zone 9", "Zone 10", "Subtropical" }, now),
            CreateCrop("Carrots", "Vegetable", 3, 1, 8, 31, 10, 70,
                new[] { "Zone 8", "Zone 9", "Subtropical" }, now),
            CreateCrop("Peppers", "Vegetable", 4, 1, 6, 30, 10, 80,
                new[] { "Zone 9", "Zone 10", "Tropical" }, now),
            CreateCrop("Strawberries", "Fruit", 3, 15, 5, 31, 14, 120,
                new[] { "Zone 8", "Zone 9", "Subtropical" }, now),
            CreateCrop("Basil", "Herb", 4, 15, 7, 31, 7, 60,
                new[] { "Zone 8", "Zone 9", "Zone 10", "Tropical", "Subtropical" }, now),
            CreateCrop("Cucumbers", "Vegetable", 4, 1, 7, 15, 7, 55,
                new[] { "Zone 8", "Zone 9", "Zone 10", "Tropical" }, now),
            CreateCrop("Beans", "Vegetable", 4, 15, 7, 31, 7, 50,
                new[] { "Zone 8", "Zone 9", "Zone 10", "Tropical", "Subtropical" }, now),
            CreateCrop("Spinach", "Vegetable", 2, 15, 4, 30, 7, 45,
                new[] { "Zone 8", "Zone 9", "Subtropical" }, now),
            CreateCrop("Zucchini", "Vegetable", 4, 15, 7, 31, 7, 50,
                new[] { "Zone 8", "Zone 9", "Zone 10", "Tropical" }, now),
            CreateCrop("Corn", "Vegetable", 4, 15, 6, 30, 10, 85,
                new[] { "Zone 8", "Zone 9", "Zone 10", "Tropical", "Subtropical" }, now),
            CreateCrop("Radishes", "Vegetable", 3, 1, 9, 30, 5, 25,
                new[] { "Zone 8", "Zone 9", "Subtropical" }, now),
            CreateCrop("Broccoli", "Vegetable", 2, 1, 4, 30, 7, 70,
                new[] { "Zone 8", "Zone 9", "Subtropical" }, now),
            CreateCrop("Watermelon", "Fruit", 5, 1, 6, 30, 10, 90,
                new[] { "Zone 9", "Zone 10", "Tropical" }, now),
            CreateCrop("Cilantro", "Herb", 3, 15, 5, 31, 7, 45,
                new[] { "Zone 8", "Zone 9", "Zone 10", "Subtropical" }, now),
            CreateCrop("Pumpkin", "Vegetable", 5, 15, 7, 15, 7, 100,
                new[] { "Zone 8", "Zone 9", "Zone 10", "Subtropical" }, now),
            CreateCrop("Kale", "Vegetable", 2, 15, 5, 31, 7, 60,
                new[] { "Zone 8", "Zone 9", "Subtropical" }, now),
            CreateCrop("Parsley", "Herb", 3, 1, 6, 30, 14, 70,
                new[] { "Zone 8", "Zone 9", "Zone 10", "Subtropical" }, now),
            CreateCrop("Eggplant", "Vegetable", 4, 15, 6, 30, 10, 85,
                new[] { "Zone 9", "Zone 10", "Tropical" }, now),
            CreateCrop("Green Onions", "Vegetable", 3, 1, 9, 30, 7, 60,
                new[] { "Zone 8", "Zone 9", "Zone 10", "Subtropical" }, now)
        };

        context.Crops.AddRange(crops);
        await context.SaveChangesAsync();
    }

    private static Crop CreateCrop(string name, string category, int startMonth, int startDay,
        int endMonth, int endDay, int germDays, int harvestDays, string[] zones, DateTime now)
    {
        return new Crop
        {
            Name = name,
            Category = category,
            Description = $"{name} - {category}",
            PlantingStartMonth = startMonth,
            PlantingStartDay = startDay,
            PlantingEndMonth = endMonth,
            PlantingEndDay = endDay,
            DaysToGermination = germDays,
            DaysToHarvest = harvestDays,
            SuitableZones = zones.ToList(),
            MinTemperature = 10,
            MaxTemperature = 35,
            SoilType = "Well-drained",
            SunRequirement = "Full Sun",
            WaterRequirement = "Medium",
            PlantingTips = $"Plant {name.ToLower()} when soil is warm",
            ImageUrl = "",
            CreatedAt = now,
            UpdatedAt = now
        };
    }
}
