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
        var crops = await _cropRepository.GetAllAsync();
        return Ok(crops);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var crop = await _cropRepository.GetByIdAsync(id);
        if (crop == null)
            return NotFound();

        return Ok(crop);
    }

    [HttpGet("by-zone/{zone}")]
    public async Task<IActionResult> GetByZone(string zone)
    {
        var crops = await _cropRepository.GetBySuitableZoneAsync(zone);
        return Ok(crops);
    }

    [HttpGet("by-category/{category}")]
    public async Task<IActionResult> GetByCategory(string category)
    {
        var crops = await _cropRepository.GetByCategoryAsync(category);
        return Ok(crops);
    }
}
