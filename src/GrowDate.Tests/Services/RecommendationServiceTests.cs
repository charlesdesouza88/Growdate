using GrowDate.Core.Entities;
using GrowDate.Core.Interfaces;
using GrowDate.Core.Services;
using Moq;
using Xunit;

namespace GrowDate.Tests.Services;

public class RecommendationServiceTests
{
    private readonly Mock<ICropRepository> _mockCropRepository;
    private readonly Mock<IRegionRepository> _mockRegionRepository;
    private readonly RecommendationService _service;

    public RecommendationServiceTests()
    {
        _mockCropRepository = new Mock<ICropRepository>();
        _mockRegionRepository = new Mock<IRegionRepository>();
        _service = new RecommendationService(_mockCropRepository.Object, _mockRegionRepository.Object);
    }

    [Fact]
    public async Task GetRecommendationsAsync_WithValidRegion_ReturnsRecommendations()
    {
        // Arrange
        var region = new Region
        {
            Id = 1,
            Name = "Test Region",
            ClimateZone = "Zone 9",
            Country = "USA",
            Latitude = 36.7783,
            Longitude = -119.4179
        };

        var crops = new List<Crop>
        {
            new Crop
            {
                Id = 1,
                Name = "Tomatoes",
                Category = "Vegetable",
                PlantingStartMonth = 3,
                PlantingStartDay = 15,
                PlantingEndMonth = 6,
                PlantingEndDay = 30,
                DaysToGermination = 7,
                DaysToHarvest = 70,
                SuitableZones = new List<string> { "Zone 9" }
            }
        };

        _mockRegionRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(region);
        _mockCropRepository.Setup(c => c.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(crops);

        var selectedDate = new DateTime(2024, 4, 15);

        // Act
        var result = await _service.GetRecommendationsAsync(1, selectedDate);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var recommendation = result.First();
        Assert.Equal("Tomatoes", recommendation.Crop.Name);
        Assert.True(recommendation.IsIdealTime);
    }

    [Fact]
    public async Task GetRecommendationsAsync_OutOfSeason_ReturnsWithCorrectStatus()
    {
        // Arrange
        var region = new Region
        {
            Id = 1,
            Name = "Test Region",
            ClimateZone = "Zone 9",
            Country = "USA",
            Latitude = 36.7783,
            Longitude = -119.4179
        };

        var crops = new List<Crop>
        {
            new Crop
            {
                Id = 1,
                Name = "Tomatoes",
                Category = "Vegetable",
                PlantingStartMonth = 3,
                PlantingStartDay = 15,
                PlantingEndMonth = 6,
                PlantingEndDay = 30,
                DaysToGermination = 7,
                DaysToHarvest = 70,
                SuitableZones = new List<string> { "Zone 9" }
            }
        };

        _mockRegionRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(region);
        _mockCropRepository.Setup(c => c.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(crops);

        var selectedDate = new DateTime(2024, 12, 15); // Out of season

        // Act
        var result = await _service.GetRecommendationsAsync(1, selectedDate);

        // Assert
        Assert.NotNull(result);
        var recommendation = result.First();
        Assert.False(recommendation.IsIdealTime);
        Assert.Equal("Out of Season", recommendation.Status);
    }

    [Fact]
    public async Task GetCropsForRegionAsync_FiltersByClimateZone()
    {
        // Arrange
        var region = new Region
        {
            Id = 1,
            Name = "Test Region",
            ClimateZone = "Zone 9",
            Country = "USA",
            Latitude = 36.7783,
            Longitude = -119.4179
        };

        var crops = new List<Crop>
        {
            new Crop
            {
                Id = 1,
                Name = "Tomatoes",
                SuitableZones = new List<string> { "Zone 9" }
            },
            new Crop
            {
                Id = 2,
                Name = "Lettuce",
                SuitableZones = new List<string> { "Zone 8" }
            }
        };

        _mockRegionRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(region);
        _mockCropRepository.Setup(c => c.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(crops);

        // Act
        var result = await _service.GetCropsForRegionAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Tomatoes", result.First().Name);
    }

    [Fact]
    public async Task GetRecommendationsAsync_WithInvalidRegion_ReturnsEmpty()
    {
        // Arrange
        _mockRegionRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Region)null);

        // Act
        var result = await _service.GetRecommendationsAsync(999, DateTime.Now);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetDetailedRecommendationAsync_CalculatesCorrectHarvestDate()
    {
        // Arrange
        var region = new Region
        {
            Id = 1,
            Name = "Test Region",
            ClimateZone = "Zone 9",
            Country = "USA",
            Latitude = 36.7783,
            Longitude = -119.4179
        };

        var crop = new Crop
        {
            Id = 1,
            Name = "Tomatoes",
            PlantingStartMonth = 3,
            PlantingStartDay = 15,
            PlantingEndMonth = 6,
            PlantingEndDay = 30,
            DaysToGermination = 7,
            DaysToHarvest = 70,
            SuitableZones = new List<string> { "Zone 9" }
        };

        _mockRegionRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(region);
        _mockCropRepository.Setup(c => c.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(crop);

        var selectedDate = new DateTime(2024, 4, 15);

        // Act
        var result = await _service.GetDetailedRecommendationAsync(1, 1, selectedDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(selectedDate.AddDays(7), result.EstimatedGerminationDate);
        Assert.Equal(selectedDate.AddDays(77), result.EstimatedHarvestDate); // 7 + 70
    }

    [Fact]
    public async Task GetRecommendationsAsync_WrapAroundSeason_TreatsEarlyYearAsInSeason()
    {
        var region = new Region
        {
            Id = 1,
            Name = "Test Region",
            ClimateZone = "Zone 9",
            Country = "USA",
            Latitude = 36.0,
            Longitude = -119.0
        };

        var crops = new List<Crop>
        {
            new Crop
            {
                Id = 1,
                Name = "WinterCrop",
                Category = "Vegetable",
                PlantingStartMonth = 11,
                PlantingStartDay = 1,
                PlantingEndMonth = 2,
                PlantingEndDay = 15,
                DaysToGermination = 7,
                DaysToHarvest = 60,
                SuitableZones = new List<string> { "Zone 9" }
            }
        };

        _mockRegionRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(region);
        _mockCropRepository.Setup(c => c.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(crops);

        var selectedDate = new DateTime(2024, 1, 10);

        var result = await _service.GetRecommendationsAsync(1, selectedDate);

        var recommendation = result.First();
        Assert.True(recommendation.IsIdealTime);
        Assert.Equal(new DateTime(2023, 11, 1), recommendation.PlantingWindowStart);
        Assert.Equal(new DateTime(2024, 2, 15), recommendation.PlantingWindowEnd);
        Assert.Equal("Ideal", recommendation.Status);
    }
}
