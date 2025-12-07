namespace GrowDate.Core.Entities;

/// <summary>
/// Represents a climate zone/region
/// </summary>
public class Region
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // Zone code (e.g., "ZONE_1", "US_MIDWEST")
    public string Country { get; set; } = string.Empty;
    public string ClimateZone { get; set; } = string.Empty; // Zone 8, Zone 9, Tropical, etc.
    public string Description { get; set; } = string.Empty;
    
    // Geographic coordinates for map display
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    // Climate characteristics
    public decimal AverageMinTemp { get; set; } // Celsius
    public decimal AverageMaxTemp { get; set; }  // Celsius
    public int FrostFreeDays { get; set; }
    public string ClimateType { get; set; } = string.Empty; // Tropical, Subtropical, Temperate, etc.
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
