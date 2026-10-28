namespace GrowDate.Core.DTOs;

public class CropDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int PlantingStartMonth { get; set; }
    public int PlantingStartDay { get; set; }
    public int PlantingEndMonth { get; set; }
    public int PlantingEndDay { get; set; }
    public int DaysToGermination { get; set; }
    public int DaysToHarvest { get; set; }
    public List<string> SuitableZones { get; set; } = new();
    public decimal MinTemperature { get; set; }
    public decimal MaxTemperature { get; set; }
    public string SoilType { get; set; } = string.Empty;
    public string SunRequirement { get; set; } = string.Empty;
    public string WaterRequirement { get; set; } = string.Empty;
    public string PlantingTips { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Spacing { get; set; } = string.Empty;
    public string Companions { get; set; } = string.Empty;
    public string Pests { get; set; } = string.Empty;
    public string Feeding { get; set; } = string.Empty;
    public string HarvestOrBloomLabel { get; set; } = "Harvest";
    public List<string> CareSteps { get; set; } = new();
}
