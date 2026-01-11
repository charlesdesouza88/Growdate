using GrowDate.Core.DTOs;
using GrowDate.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GrowDate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private readonly IRecommendationService _recommendationService;
    private readonly IRegionRepository _regionRepository;

    public RecommendationsController(IRecommendationService recommendationService, IRegionRepository regionRepository)
    {
        _recommendationService = recommendationService;
        _regionRepository = regionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecommendations(
        [FromQuery] int regionId,
        [FromQuery] DateTime? selectedDate = null)
    {
        if (regionId <= 0)
            return BadRequest("regionId must be provided.");

        var region = await _regionRepository.GetByIdAsync(regionId, HttpContext.RequestAborted);
        if (region == null)
            return NotFound($"Region {regionId} was not found.");

        var date = (selectedDate ?? DateTime.UtcNow).Date;
        var recommendations = await _recommendationService.GetRecommendationsAsync(regionId, date, HttpContext.RequestAborted);
        return Ok(recommendations.Select(r => r.ToDto()));
    }

    [HttpGet("crops")]
    public async Task<IActionResult> GetCropsForRegion([FromQuery] int regionId)
    {
        if (regionId <= 0)
            return BadRequest("regionId must be provided.");

        var region = await _regionRepository.GetByIdAsync(regionId, HttpContext.RequestAborted);
        if (region == null)
            return NotFound($"Region {regionId} was not found.");

        var crops = await _recommendationService.GetCropsForRegionAsync(regionId, HttpContext.RequestAborted);
        return Ok(crops.Select(c => c.ToDto()));
    }

    [HttpGet("date-range")]
    public async Task<IActionResult> GetCropsForDateRange(
        [FromQuery] int regionId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        if (regionId <= 0)
            return BadRequest("regionId must be provided.");

        if (startDate > endDate)
            return BadRequest("startDate must be before or equal to endDate.");

        var region = await _regionRepository.GetByIdAsync(regionId, HttpContext.RequestAborted);
        if (region == null)
            return NotFound($"Region {regionId} was not found.");

        var crops = await _recommendationService.GetCropsForDateRangeAsync(regionId, startDate, endDate, HttpContext.RequestAborted);
        return Ok(crops.Select(c => c.ToDto()));
    }

    [HttpGet("detailed")]
    public async Task<IActionResult> GetDetailedRecommendation(
        [FromQuery] int cropId,
        [FromQuery] int regionId,
        [FromQuery] DateTime? selectedDate = null)
    {
        if (regionId <= 0 || cropId <= 0)
            return BadRequest("cropId and regionId must be provided.");

        var region = await _regionRepository.GetByIdAsync(regionId, HttpContext.RequestAborted);
        if (region == null)
            return NotFound($"Region {regionId} was not found.");

        var date = (selectedDate ?? DateTime.UtcNow).Date;
        var recommendation = await _recommendationService.GetDetailedRecommendationAsync(cropId, regionId, date, HttpContext.RequestAborted);
        
        if (recommendation == null)
            return NotFound();

        return Ok(recommendation.ToDto());
    }
}
