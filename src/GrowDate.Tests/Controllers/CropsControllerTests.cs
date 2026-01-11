using GrowDate.Core.DTOs;
using GrowDate.Core.Entities;
using GrowDate.Core.Interfaces;
using GrowDate.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace GrowDate.Tests.Controllers;

public class CropsControllerTests
{
    private readonly Mock<ICropRepository> _mockCropRepository;
    private readonly CropsController _controller;

    public CropsControllerTests()
    {
        _mockCropRepository = new Mock<ICropRepository>();
        _controller = new CropsController(_mockCropRepository.Object);

        // Setup mock HttpContext with RequestAborted token
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext.HttpContext = httpContext;
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithCrops()
    {
        // Arrange
        var crops = new[]
        {
            new Crop { Id = 1, Name = "Tomatoes", Category = "Vegetable" },
            new Crop { Id = 2, Name = "Lettuce", Category = "Vegetable" }
        };

        _mockCropRepository
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(crops);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var returnedData = Assert.IsAssignableFrom<IEnumerable<CropDto>>(okResult.Value);
        Assert.Equal(2, returnedData.Count());
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkWithCrop()
    {
        // Arrange
        var cropId = 1;
        var crop = new Crop { Id = cropId, Name = "Tomatoes", Category = "Vegetable" };

        _mockCropRepository
            .Setup(r => r.GetByIdAsync(cropId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(crop);

        // Act
        var result = await _controller.GetById(cropId);

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var returnedData = Assert.IsType<CropDto>(okResult.Value);
        Assert.Equal("Tomatoes", returnedData.Name);
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
        var cropId = 999;
        _mockCropRepository
            .Setup(r => r.GetByIdAsync(cropId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Crop)null);

        // Act
        var result = await _controller.GetById(cropId);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByZone_WithValidZone_ReturnsOkWithCrops()
    {
        // Arrange
        var zone = "Zone 9";
        var crops = new[]
        {
            new Crop { Id = 1, Name = "Tomatoes", SuitableZones = new List<string> { "Zone 9" } }
        };

        _mockCropRepository
            .Setup(r => r.GetBySuitableZoneAsync(zone, It.IsAny<CancellationToken>()))
            .ReturnsAsync(crops);

        // Act
        var result = await _controller.GetByZone(zone);

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var returnedData = Assert.IsAssignableFrom<IEnumerable<CropDto>>(okResult.Value);
        Assert.Single(returnedData);
    }

    [Fact]
    public async Task GetByZone_WithBlankZone_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetByZone("");

        // Assert
        var badRequest = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
        Assert.NotNull(badRequest.Value);
    }

    [Fact]
    public async Task GetByCategory_WithValidCategory_ReturnsOkWithCrops()
    {
        // Arrange
        var category = "Vegetable";
        var crops = new[]
        {
            new Crop { Id = 1, Name = "Tomatoes", Category = "Vegetable" }
        };

        _mockCropRepository
            .Setup(r => r.GetByCategoryAsync(category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(crops);

        // Act
        var result = await _controller.GetByCategory(category);

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var returnedData = Assert.IsAssignableFrom<IEnumerable<CropDto>>(okResult.Value);
        Assert.Single(returnedData);
    }
}
