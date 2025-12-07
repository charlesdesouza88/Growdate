using GrowDate.Core.Entities;
using GrowDate.Infrastructure.Data;
using GrowDate.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GrowDate.Tests.Repositories;

public class CropRepositoryTests
{
    private GrowDateDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<GrowDateDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new GrowDateDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCrops()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new CropRepository(context);

        var crops = new List<Crop>
        {
            new Crop { Name = "Tomatoes", Category = "Vegetable", SuitableZones = new List<string> { "Zone 9" } },
            new Crop { Name = "Lettuce", Category = "Vegetable", SuitableZones = new List<string> { "Zone 8" } }
        };

        context.Crops.AddRange(crops);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCrop()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new CropRepository(context);

        var crop = new Crop
        {
            Name = "Tomatoes",
            Category = "Vegetable",
            SuitableZones = new List<string> { "Zone 9" }
        };

        context.Crops.Add(crop);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(crop.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Tomatoes", result.Name);
    }

    [Fact]
    public async Task GetByCategoryAsync_FiltersByCategory()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new CropRepository(context);

        var crops = new List<Crop>
        {
            new Crop { Name = "Tomatoes", Category = "Vegetable", SuitableZones = new List<string>() },
            new Crop { Name = "Basil", Category = "Herb", SuitableZones = new List<string>() },
            new Crop { Name = "Lettuce", Category = "Vegetable", SuitableZones = new List<string>() }
        };

        context.Crops.AddRange(crops);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByCategoryAsync("Vegetable");

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, c => Assert.Equal("Vegetable", c.Category));
    }

    [Fact]
    public async Task GetBySuitableZoneAsync_FiltersByZone()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new CropRepository(context);

        var crops = new List<Crop>
        {
            new Crop { Name = "Tomatoes", SuitableZones = new List<string> { "Zone 9", "Zone 10" } },
            new Crop { Name = "Lettuce", SuitableZones = new List<string> { "Zone 8" } },
            new Crop { Name = "Peppers", SuitableZones = new List<string> { "Zone 9" } }
        };

        context.Crops.AddRange(crops);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetBySuitableZoneAsync("Zone 9");

        // Assert
        Assert.Equal(2, result.Count());
    }
}
