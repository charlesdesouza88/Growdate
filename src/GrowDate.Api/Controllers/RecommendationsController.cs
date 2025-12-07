using GrowDate.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GrowDate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private readonly IRecommendationService _recommendationService;

    public RecommendationsController(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecommendations(
        [FromQuery] int regionId,
        [FromQuery] DateTime? selectedDate = null)
    {
        var date = selectedDate ?? DateTime.Now;
        var recommendations = await _recommendationService.GetRecommendationsAsync(regionId, date);
        return Ok(recommendations);
    }

    [HttpGet("crops")]
    public async Task<IActionResult> GetCropsForRegion([FromQuery] int regionId)
    {
        var crops = await _recommendationService.GetCropsForRegionAsync(regionId);
        return Ok(crops);
    }

    [HttpGet("date-range")]
    public async Task<IActionResult> GetCropsForDateRange(
        [FromQuery] int regionId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var crops = await _recommendationService.GetCropsForDateRangeAsync(regionId, startDate, endDate);
        return Ok(crops);
    }

    [HttpGet("detailed")]
    public async Task<IActionResult> GetDetailedRecommendation(
        [FromQuery] int cropId,
        [FromQuery] int regionId,
        [FromQuery] DateTime? selectedDate = null)
    {
        var date = selectedDate ?? DateTime.Now;
        var recommendation = await _recommendationService.GetDetailedRecommendationAsync(cropId, regionId, date);
        
        if (recommendation == null)
            return NotFound();

        return Ok(recommendation);
    }
}
