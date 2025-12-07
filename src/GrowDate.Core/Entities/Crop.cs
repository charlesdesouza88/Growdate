namespace GrowDate.Core.Entities;

/// <summary>
/// Represents a crop that can be planted
/// </summary>
public class Crop
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Vegetable, Fruit, Herb
    
    // Planting window - month and day ranges
    public int PlantingStartMonth { get; set; }
    public int PlantingStartDay { get; set; }
    public int PlantingEndMonth { get; set; }
    public int PlantingEndDay { get; set; }
    
    // Growth timeline
    public int DaysToGermination { get; set; }
    public int DaysToHarvest { get; set; }
    
    // Climate requirements
    public List<string> SuitableZones { get; set; } = new List<string>(); // Zone 8, Zone 9, Tropical, etc.
    public string ClimateZones { get; set; } = string.Empty; // Comma-separated zone IDs (legacy)
    public decimal MinTemperature { get; set; } // Celsius
    public decimal MaxTemperature { get; set; } // Celsius
    
    // Growing conditions
    public string SoilType { get; set; } = string.Empty;
    public string SunRequirement { get; set; } = string.Empty; // Full Sun, Partial Shade, Shade
    public string WaterRequirement { get; set; } = string.Empty; // Low, Medium, High
    
    // Additional info
    public string PlantingTips { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
