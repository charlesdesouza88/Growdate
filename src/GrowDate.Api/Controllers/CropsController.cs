using GrowDate.Core.DTOs;
using GrowDate.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GrowDate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CropsController : ControllerBase
{
    private readonly ICropRepository _cropRepository;

    public CropsController(ICropRepository cropRepository)
    {
        _cropRepository = cropRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var crops = await _cropRepository.GetAllAsync(HttpContext.RequestAborted);
        return Ok(crops.Select(c => c.ToDto()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("id must be positive.");

        var crop = await _cropRepository.GetByIdAsync(id, HttpContext.RequestAborted);
        if (crop == null)
            return NotFound();

        return Ok(crop.ToDto());
    }

    [HttpGet("by-zone/{zone}")]
    public async Task<IActionResult> GetByZone(string zone)
    {
        if (string.IsNullOrWhiteSpace(zone))
            return BadRequest("zone must be provided.");

        var crops = await _cropRepository.GetBySuitableZoneAsync(zone, HttpContext.RequestAborted);
        return Ok(crops.Select(c => c.ToDto()));
    }

    [HttpGet("by-category/{category}")]
    public async Task<IActionResult> GetByCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return BadRequest("category must be provided.");

        var crops = await _cropRepository.GetByCategoryAsync(category, HttpContext.RequestAborted);
        return Ok(crops.Select(c => c.ToDto()));
    }
}
