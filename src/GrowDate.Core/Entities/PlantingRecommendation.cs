namespace GrowDate.Core.Entities;

/// <summary>
/// Represents a planting recommendation for a specific crop, region, and date
/// </summary>
public class PlantingRecommendation
{
    public Crop Crop { get; set; } = null!;
    public Region Region { get; set; } = null!;
    public DateTime SelectedDate { get; set; }
    public bool IsIdealTime { get; set; }
    public DateTime PlantingWindowStart { get; set; }
    public DateTime PlantingWindowEnd { get; set; }
    public DateTime EstimatedGerminationDate { get; set; }
    public DateTime EstimatedHarvestDate { get; set; }
    public string Status { get; set; } = string.Empty; // "Ideal", "Coming Soon", "Late Season", "Out of Season"
    public string Notes { get; set; } = string.Empty;
}
