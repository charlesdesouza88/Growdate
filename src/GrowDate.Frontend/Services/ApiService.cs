using System.Net.Http.Json;
using GrowDate.Core.DTOs;
using Microsoft.Extensions.Logging;

namespace GrowDate.Frontend.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiService> _logger;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<RegionDto>> GetRegionsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/regions");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("GET api/regions failed with {StatusCode}", response.StatusCode);
                return new List<RegionDto>();
            }

            return await response.Content.ReadFromJsonAsync<List<RegionDto>>() ?? new List<RegionDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load regions from {BaseAddress}", _httpClient.BaseAddress);
            return new List<RegionDto>();
        }
    }

    public async Task<RegionDto?> GetRegionByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<RegionDto>($"api/regions/{id}");
    }

    public async Task<List<CropDto>> GetCropsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CropDto>>("api/crops") ?? new List<CropDto>();
    }

    public async Task<CropDto?> GetCropByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<CropDto>($"api/crops/{id}");
    }

    public async Task<List<CropDto>> GetCropsByZoneAsync(string zone)
    {
        return await _httpClient.GetFromJsonAsync<List<CropDto>>(
            $"api/crops/by-zone/{Uri.EscapeDataString(zone)}") ?? new List<CropDto>();
    }

    public async Task<List<RecommendationDto>> GetRecommendationsAsync(int regionId, DateTime selectedDate)
    {
        var dateStr = selectedDate.ToString("yyyy-MM-dd");
        return await _httpClient.GetFromJsonAsync<List<RecommendationDto>>(
            $"api/recommendations?regionId={regionId}&selectedDate={dateStr}") ?? new List<RecommendationDto>();
    }

    public async Task<List<CropDto>> GetCropsForRegionAsync(int regionId)
    {
        return await _httpClient.GetFromJsonAsync<List<CropDto>>(
            $"api/recommendations/crops?regionId={regionId}") ?? new List<CropDto>();
    }

    public async Task<List<CropDto>> GetCropsForDateRangeAsync(int regionId, DateTime startDate, DateTime endDate)
    {
        var start = startDate.ToString("yyyy-MM-dd");
        var end = endDate.ToString("yyyy-MM-dd");
        return await _httpClient.GetFromJsonAsync<List<CropDto>>(
            $"api/recommendations/date-range?regionId={regionId}&startDate={start}&endDate={end}") ?? new List<CropDto>();
    }

    public async Task<RecommendationDto?> GetDetailedRecommendationAsync(int cropId, int regionId, DateTime selectedDate)
    {
        var dateStr = selectedDate.ToString("yyyy-MM-dd");
        return await _httpClient.GetFromJsonAsync<RecommendationDto>(
            $"api/recommendations/detailed?cropId={cropId}&regionId={regionId}&selectedDate={dateStr}");
    }
}
