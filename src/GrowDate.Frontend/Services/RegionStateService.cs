using GrowDate.Core.DTOs;

namespace GrowDate.Frontend.Services;

public class RegionStateService
{
    private const string RegionStorageKey = "selectedRegion";
    private RegionDto? _selectedRegion;

    public RegionDto? SelectedRegion
    {
        get => _selectedRegion;
        set => _selectedRegion = value;
    }

    public void SetRegion(RegionDto? region)
    {
        _selectedRegion = region;
    }

    public RegionDto? GetRegion()
    {
        return _selectedRegion;
    }

    public void ClearRegion()
    {
        _selectedRegion = null;
    }
}
