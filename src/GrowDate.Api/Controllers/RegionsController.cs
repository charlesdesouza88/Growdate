using GrowDate.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GrowDate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionsController : ControllerBase
{
    private readonly IRegionRepository _regionRepository;

    public RegionsController(IRegionRepository regionRepository)
    {
        _regionRepository = regionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var regions = await _regionRepository.GetAllAsync();
        return Ok(regions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var region = await _regionRepository.GetByIdAsync(id);
        if (region == null)
            return NotFound();

        return Ok(region);
    }

    [HttpGet("by-country/{country}")]
    public async Task<IActionResult> GetByCountry(string country)
    {
        var regions = await _regionRepository.GetByCountryAsync(country);
        return Ok(regions);
    }

    [HttpGet("by-zone/{zone}")]
    public async Task<IActionResult> GetByClimateZone(string zone)
    {
        var regions = await _regionRepository.GetByClimateZoneAsync(zone);
        return Ok(regions);
    }
}
