using System.Net.Http.Json;
using GrowDate.Core.DTOs;

namespace GrowDate.Frontend.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Regions
    public async Task<List<RegionDto>> GetRegionsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<RegionDto>>("api/regions") ?? new List<RegionDto>();
    }

    public async Task<RegionDto?> GetRegionByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<RegionDto>($"api/regions/{id}");
    }

    // Crops
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
        return await _httpClient.GetFromJsonAsync<List<CropDto>>($"api/crops/by-zone/{zone}") ?? new List<CropDto>();
    }

    // Recommendations
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

    public async Task<RecommendationDto?> GetDetailedRecommendationAsync(int cropId, int regionId, DateTime selectedDate)
    {
        var dateStr = selectedDate.ToString("yyyy-MM-dd");
        return await _httpClient.GetFromJsonAsync<RecommendationDto>(
            $"api/recommendations/detailed?cropId={cropId}&regionId={regionId}&selectedDate={dateStr}");
    }
}
