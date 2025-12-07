namespace GrowDate.Core.DTOs;

public class RecommendationDto
{
    public CropDto Crop { get; set; }
    public RegionDto Region { get; set; }
    public DateTime SelectedDate { get; set; }
    public bool IsIdealTime { get; set; }
    public DateTime PlantingWindowStart { get; set; }
    public DateTime PlantingWindowEnd { get; set; }
    public DateTime EstimatedGerminationDate { get; set; }
    public DateTime EstimatedHarvestDate { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
}
