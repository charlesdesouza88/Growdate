using GrowDate.Core.DTOs;
using GrowDate.Core.Entities;
using GrowDate.Core.Interfaces;
using GrowDate.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace GrowDate.Tests.Controllers;

public class RegionsControllerTests
{
    private readonly Mock<IRegionRepository> _mockRegionRepository;
    private readonly RegionsController _controller;

    public RegionsControllerTests()
    {
        _mockRegionRepository = new Mock<IRegionRepository>();
        _controller = new RegionsController(_mockRegionRepository.Object);

        // Setup mock HttpContext with RequestAborted token
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext.HttpContext = httpContext;
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithRegions()
    {
        // Arrange
        var regions = new[]
        {
            new Region { Id = 1, Name = "California", Country = "USA", ClimateZone = "Zone 9" },
            new Region { Id = 2, Name = "Florida", Country = "USA", ClimateZone = "Zone 10" }
        };

        _mockRegionRepository
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(regions);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var returnedData = Assert.IsAssignableFrom<IEnumerable<RegionDto>>(okResult.Value);
        Assert.Equal(2, returnedData.Count());
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkWithRegion()
    {
        // Arrange
        var regionId = 1;
        var region = new Region { Id = regionId, Name = "California", Country = "USA", ClimateZone = "Zone 9" };

        _mockRegionRepository
            .Setup(r => r.GetByIdAsync(regionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(region);

        // Act
        var result = await _controller.GetById(regionId);

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var returnedData = Assert.IsType<RegionDto>(okResult.Value);
        Assert.Equal("California", returnedData.Name);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetById(0);

        // Assert
        var badRequest = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
        Assert.NotNull(badRequest.Value);
    }

    [Fact]
    public async Task GetById_WithNonexistentId_ReturnsNotFound()
    {
        // Arrange
        var regionId = 999;
        _mockRegionRepository
            .Setup(r => r.GetByIdAsync(regionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Region)null);

        // Act
        var result = await _controller.GetById(regionId);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByCountry_WithValidCountry_ReturnsOkWithRegions()
    {
        // Arrange
        var country = "USA";
        var regions = new[]
        {
            new Region { Id = 1, Name = "California", Country = "USA", ClimateZone = "Zone 9" }
        };

        _mockRegionRepository
            .Setup(r => r.GetByCountryAsync(country, It.IsAny<CancellationToken>()))
            .ReturnsAsync(regions);

        // Act
        var result = await _controller.GetByCountry(country);

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var returnedData = Assert.IsAssignableFrom<IEnumerable<RegionDto>>(okResult.Value);
        Assert.Single(returnedData);
    }

    [Fact]
    public async Task GetByCountry_WithBlankCountry_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetByCountry("");

        // Assert
        var badRequest = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
        Assert.NotNull(badRequest.Value);
    }

    [Fact]
    public async Task GetByClimateZone_WithValidZone_ReturnsOkWithRegions()
    {
        // Arrange
        var zone = "Zone 9";
        var regions = new[]
        {
            new Region { Id = 1, Name = "California", Country = "USA", ClimateZone = "Zone 9" }
        };

        _mockRegionRepository
            .Setup(r => r.GetByClimateZoneAsync(zone, It.IsAny<CancellationToken>()))
            .ReturnsAsync(regions);

        // Act
        var result = await _controller.GetByClimateZone(zone);

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var returnedData = Assert.IsAssignableFrom<IEnumerable<RegionDto>>(okResult.Value);
        Assert.Single(returnedData);
    }
}
