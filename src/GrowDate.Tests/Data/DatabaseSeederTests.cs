using GrowDate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GrowDate.Tests.Data;

public class DatabaseSeederTests
{
    private static GrowDateDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<GrowDateDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new GrowDateDbContext(options);
    }

    [Fact]
    public void LoadCatalog_ReturnsFiftyPlusPlantsAcrossFourCategories()
    {
        var catalog = DatabaseSeeder.LoadCatalog();

        Assert.Equal(2, catalog.SeedVersion);
        Assert.True(catalog.Crops.Count >= 50);
        Assert.Contains(catalog.Crops, c => c.Category == "Vegetable");
        Assert.Contains(catalog.Crops, c => c.Category == "Fruit");
        Assert.Contains(catalog.Crops, c => c.Category == "Herb");
        Assert.Contains(catalog.Crops, c => c.Category == "Flower");
        Assert.All(catalog.Crops.Where(c => c.Category == "Flower"),
            c => Assert.Equal("Bloom", c.HarvestOrBloomLabel));
        Assert.Contains(catalog.Crops, c => c.Name == "Tomatoes" && !string.IsNullOrWhiteSpace(c.ImageUrl));
        Assert.Contains(catalog.Crops, c => c.Name == "Sunflower");
    }

    [Fact]
    public async Task SeedAsync_UpsertsWithoutDuplicatingCrops()
    {
        await using var context = CreateContext();

        await DatabaseSeeder.SeedAsync(context);
        var firstCount = context.Crops.Count();
        var firstVersion = context.SeedMeta.Single().Version;

        await DatabaseSeeder.SeedAsync(context);
        var secondCount = context.Crops.Count();

        Assert.True(firstCount >= 50);
        Assert.Equal(firstCount, secondCount);
        Assert.Equal(DatabaseSeeder.CurrentSeedVersion, firstVersion);
        Assert.Equal(6, context.Regions.Count());
        Assert.Contains(context.Crops, c => c.Category == "Flower" && c.HarvestOrBloomLabel == "Bloom");
        var tomatoes = context.Crops.Single(c => c.Name == "Tomatoes");
        Assert.Contains("Basil", tomatoes.Companions);
        Assert.False(string.IsNullOrWhiteSpace(tomatoes.ImageUrl));
        Assert.NotEmpty(tomatoes.CareSteps);
    }
}
