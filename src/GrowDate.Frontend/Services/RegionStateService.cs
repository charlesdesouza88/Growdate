using System.Text.Json;
using GrowDate.Core.DTOs;
using Microsoft.JSInterop;

namespace GrowDate.Frontend.Services;

public class RegionStateService
{
    private const string StorageKey = "growdate.selectedRegion";
    private readonly IJSRuntime _js;
    private RegionDto? _selectedRegion;
    private bool _loaded;

    public RegionStateService(IJSRuntime js)
    {
        _js = js;
    }

    public RegionDto? SelectedRegion => _selectedRegion;

    public event Action? Changed;

    public async Task EnsureLoadedAsync()
    {
        if (_loaded)
            return;

        try
        {
            var json = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrWhiteSpace(json))
                _selectedRegion = JsonSerializer.Deserialize<RegionDto>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not restore region: {ex.Message}");
        }

        _loaded = true;
        Changed?.Invoke();
    }

    public void SetRegion(RegionDto? region)
    {
        _selectedRegion = region;
        _ = PersistAsync();
        Changed?.Invoke();
    }

    public RegionDto? GetRegion() => _selectedRegion;

    public void ClearRegion()
    {
        _selectedRegion = null;
        _ = PersistAsync();
        Changed?.Invoke();
    }

    private async Task PersistAsync()
    {
        try
        {
            if (_selectedRegion == null)
            {
                await _js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
                return;
            }

            var json = JsonSerializer.Serialize(_selectedRegion);
            await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not persist region: {ex.Message}");
        }
    }
}
