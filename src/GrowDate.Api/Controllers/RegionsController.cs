using GrowDate.Core.DTOs;
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
        var regions = await _regionRepository.GetAllAsync(HttpContext.RequestAborted);
        return Ok(regions.Select(r => r.ToDto()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("id must be positive.");

        var region = await _regionRepository.GetByIdAsync(id, HttpContext.RequestAborted);
        if (region == null)
            return NotFound();

        return Ok(region.ToDto());
    }

    [HttpGet("by-country/{country}")]
    public async Task<IActionResult> GetByCountry(string country)
    {
        if (string.IsNullOrWhiteSpace(country))
            return BadRequest("country must be provided.");

        var regions = await _regionRepository.GetByCountryAsync(country, HttpContext.RequestAborted);
        return Ok(regions.Select(r => r.ToDto()));
    }

    [HttpGet("by-zone/{zone}")]
    public async Task<IActionResult> GetByClimateZone(string zone)
    {
        if (string.IsNullOrWhiteSpace(zone))
            return BadRequest("zone must be provided.");

        var regions = await _regionRepository.GetByClimateZoneAsync(zone, HttpContext.RequestAborted);
        return Ok(regions.Select(r => r.ToDto()));
    }
}
