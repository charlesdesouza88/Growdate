namespace GrowDate.Core.DTOs;

public class CropDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public int PlantingStartMonth { get; set; }
    public int PlantingStartDay { get; set; }
    public int PlantingEndMonth { get; set; }
    public int PlantingEndDay { get; set; }
    public int DaysToGermination { get; set; }
    public int DaysToHarvest { get; set; }
    public List<string> SuitableZones { get; set; }
}
