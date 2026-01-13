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
        try
        {
            Console.WriteLine($"🌐 Making API call to: {_httpClient.BaseAddress}api/regions");
            var response = await _httpClient.GetAsync("api/regions");
            Console.WriteLine($"📡 API Response Status: {response.StatusCode}");
            
            if (response.IsSuccessStatusCode)
            {
                var regions = await response.Content.ReadFromJsonAsync<List<RegionDto>>();
                Console.WriteLine($"✅ Successfully parsed {regions?.Count ?? 0} regions from API");
                return regions ?? new List<RegionDto>();
            }
            else
            {
                Console.WriteLine($"❌ API call failed with status: {response.StatusCode}");
                Console.WriteLine($"📄 Response content: {await response.Content.ReadAsStringAsync()}");
                return new List<RegionDto>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception in GetRegionsAsync: {ex.Message}");
            Console.WriteLine($"🔍 Exception type: {ex.GetType().Name}");
            throw;
        }
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
