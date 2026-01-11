namespace GrowDate.Core.DTOs;

public class RegionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string ClimateZone { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
