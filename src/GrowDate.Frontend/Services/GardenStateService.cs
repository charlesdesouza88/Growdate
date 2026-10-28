using System.Text.Json;
using Microsoft.JSInterop;

namespace GrowDate.Frontend.Services;

public class GardenStateService
{
    private const string StorageKey = "growdate.gardenCropIds";
    private readonly IJSRuntime _js;
    private readonly HashSet<int> _ids = new();
    private bool _loaded;

    public GardenStateService(IJSRuntime js)
    {
        _js = js;
    }

    public IReadOnlyCollection<int> CropIds => _ids;

    public async Task EnsureLoadedAsync()
    {
        if (_loaded)
            return;

        try
        {
            var json = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrWhiteSpace(json))
            {
                var ids = JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
                _ids.Clear();
                foreach (var id in ids)
                    _ids.Add(id);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not restore garden: {ex.Message}");
        }

        _loaded = true;
    }

    public bool Contains(int cropId) => _ids.Contains(cropId);

    public void Add(int cropId)
    {
        if (_ids.Add(cropId))
            _ = PersistAsync();
    }

    public void Remove(int cropId)
    {
        if (_ids.Remove(cropId))
            _ = PersistAsync();
    }

    public void Toggle(int cropId)
    {
        if (!_ids.Remove(cropId))
            _ids.Add(cropId);
        _ = PersistAsync();
    }

    private async Task PersistAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_ids.ToList());
            await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not persist garden: {ex.Message}");
        }
    }
}
