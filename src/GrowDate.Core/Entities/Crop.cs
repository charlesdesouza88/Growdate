using System.ComponentModel.DataAnnotations.Schema;

namespace GrowDate.Core.Entities;

/// <summary>
/// A plant in the GrowDate catalog — vegetables, fruits, herbs, and flowers.
/// </summary>
public class Crop
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Vegetable, Fruit, Herb, Flower

    public int PlantingStartMonth { get; set; }
    public int PlantingStartDay { get; set; }
    public int PlantingEndMonth { get; set; }
    public int PlantingEndDay { get; set; }

    public int DaysToGermination { get; set; }
    public int DaysToHarvest { get; set; }

    public string SuitableZonesCsv { get; set; } = string.Empty;

    [NotMapped]
    public List<string> SuitableZones
    {
        get => SuitableZonesCsv.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(z => z.Trim())
            .Where(z => !string.IsNullOrWhiteSpace(z))
            .ToList();
        set => SuitableZonesCsv = value == null || value.Count == 0
            ? string.Empty
            : string.Join(',', value);
    }

    public string ClimateZones { get; set; } = string.Empty;
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

    public string CareStepsText { get; set; } = string.Empty;

    [NotMapped]
    public List<string> CareSteps
    {
        get => CareStepsText
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .ToList();
        set => CareStepsText = value == null || value.Count == 0
            ? string.Empty
            : string.Join('\n', value);
    }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
