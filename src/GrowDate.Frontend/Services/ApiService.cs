using System.Net.Http.Json;
using GrowDate.Core.Entities;

namespace GrowDate.Frontend.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Regions
    public async Task<List<Region>> GetRegionsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Region>>("api/regions") ?? new List<Region>();
    }

    public async Task<Region?> GetRegionByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Region>($"api/regions/{id}");
    }

    // Crops
    public async Task<List<Crop>> GetCropsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Crop>>("api/crops") ?? new List<Crop>();
    }

    public async Task<Crop?> GetCropByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Crop>($"api/crops/{id}");
    }

    public async Task<List<Crop>> GetCropsByZoneAsync(string zone)
    {
        return await _httpClient.GetFromJsonAsync<List<Crop>>($"api/crops/by-zone/{zone}") ?? new List<Crop>();
    }

    // Recommendations
    public async Task<List<PlantingRecommendation>> GetRecommendationsAsync(int regionId, DateTime selectedDate)
    {
        var dateStr = selectedDate.ToString("yyyy-MM-dd");
        return await _httpClient.GetFromJsonAsync<List<PlantingRecommendation>>(
            $"api/recommendations?regionId={regionId}&selectedDate={dateStr}") ?? new List<PlantingRecommendation>();
    }

    public async Task<List<Crop>> GetCropsForRegionAsync(int regionId)
    {
        return await _httpClient.GetFromJsonAsync<List<Crop>>(
            $"api/recommendations/crops?regionId={regionId}") ?? new List<Crop>();
    }

    public async Task<PlantingRecommendation?> GetDetailedRecommendationAsync(int cropId, int regionId, DateTime selectedDate)
    {
        var dateStr = selectedDate.ToString("yyyy-MM-dd");
        return await _httpClient.GetFromJsonAsync<PlantingRecommendation>(
            $"api/recommendations/detailed?cropId={cropId}&regionId={regionId}&selectedDate={dateStr}");
    }
}
