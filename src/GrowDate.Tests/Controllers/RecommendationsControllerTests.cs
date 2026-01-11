using GrowDate.Core.DTOs;
using GrowDate.Core.Entities;
using GrowDate.Core.Interfaces;
using GrowDate.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace GrowDate.Tests.Controllers;

public class RecommendationsControllerTests
{
    private readonly Mock<IRecommendationService> _mockRecommendationService;
    private readonly Mock<IRegionRepository> _mockRegionRepository;
    private readonly RecommendationsController _controller;

    public RecommendationsControllerTests()
    {
        _mockRecommendationService = new Mock<IRecommendationService>();
        _mockRegionRepository = new Mock<IRegionRepository>();
        _controller = new RecommendationsController(_mockRecommendationService.Object, _mockRegionRepository.Object);

        // Setup mock HttpContext with RequestAborted token
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext.HttpContext = httpContext;
    }

    [Fact]
    public async Task GetRecommendations_WithValidRegionAndDate_ReturnsOkWithRecommendations()
    {
        // Arrange
        var regionId = 1;
        var selectedDate = new DateTime(2024, 4, 15);
        var region = new Region { Id = regionId, Name = "Test Region", ClimateZone = "Zone 9" };
        var crop = new Crop
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
        };
        var recommendation = new PlantingRecommendation
        {
            Crop = crop,
            Region = region,
            SelectedDate = selectedDate,
            IsIdealTime = true,
            PlantingWindowStart = new DateTime(2024, 3, 15),
            PlantingWindowEnd = new DateTime(2024, 6, 30),
            EstimatedGerminationDate = selectedDate.AddDays(7),
            EstimatedHarvestDate = selectedDate.AddDays(77),
            Status = "Ideal",
            Notes = "Perfect time to plant"
        };

        _mockRegionRepository
            .Setup(r => r.GetByIdAsync(regionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(region);
        _mockRecommendationService
            .Setup(s => s.GetRecommendationsAsync(regionId, selectedDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { recommendation });

        // Act
        var result = await _controller.GetRecommendations(regionId, selectedDate);

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var returnedData = Assert.IsAssignableFrom<IEnumerable<RecommendationDto>>(okResult.Value);
        Assert.Single(returnedData);
    }

    [Fact]
    public async Task GetRecommendations_WithInvalidRegionId_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetRecommendations(0);

        // Assert
        var badRequest = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
        Assert.NotNull(badRequest.Value);
    }

    [Fact]
    public async Task GetRecommendations_WithNonexistentRegion_ReturnsNotFound()
    {
        // Arrange
        var regionId = 999;
        _mockRegionRepository
            .Setup(r => r.GetByIdAsync(regionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Region)null);

        // Act
        var result = await _controller.GetRecommendations(regionId);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetCropsForDateRange_WithInvalidDateRange_ReturnsBadRequest()
    {
        // Arrange
        var regionId = 1;
        var startDate = new DateTime(2024, 6, 1);
        var endDate = new DateTime(2024, 3, 1);

        // Act
        var result = await _controller.GetCropsForDateRange(regionId, startDate, endDate);

        // Assert
        var badRequest = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
        Assert.NotNull(badRequest.Value);
    }

    [Fact]
    public async Task GetCropsForRegion_WithValidRegion_ReturnsOkWithCrops()
    {
        // Arrange
        var regionId = 1;
        var region = new Region { Id = regionId, Name = "Test Region", ClimateZone = "Zone 9" };
        var crops = new[]
        {
            new Crop { Id = 1, Name = "Tomatoes", SuitableZones = new List<string> { "Zone 9" } }
        };

        _mockRegionRepository
            .Setup(r => r.GetByIdAsync(regionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(region);
        _mockRecommendationService
            .Setup(s => s.GetCropsForRegionAsync(regionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(crops);

        // Act
        var result = await _controller.GetCropsForRegion(regionId);

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var returnedData = Assert.IsAssignableFrom<IEnumerable<CropDto>>(okResult.Value);
        Assert.Single(returnedData);
    }
}
