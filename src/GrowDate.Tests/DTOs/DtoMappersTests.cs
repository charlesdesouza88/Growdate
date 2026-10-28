using GrowDate.Core.DTOs;
using GrowDate.Core.Entities;
using Xunit;

namespace GrowDate.Tests.DTOs;

public class DtoMappersTests
{
    [Fact]
    public void CropToDto_IncludesCareAndImageFields()
    {
        var crop = new Crop
        {
            Id = 7,
            Name = "Sunflower",
            Slug = "sunflower",
            Category = "Flower",
            Description = "Tall annuals.",
            ImageUrl = "/images/crops/sunflower.svg",
            SunRequirement = "Full Sun",
            WaterRequirement = "Medium",
            SoilType = "Average soil",
            Spacing = "30 cm",
            Companions = "Corn",
            Pests = "Birds",
            Feeding = "Compost at sowing.",
            HarvestOrBloomLabel = "Bloom",
            CareSteps = new List<string> { "Sow after frost.", "Stake tall types." },
            SuitableZones = new List<string> { "Zone 9" }
        };

        var dto = crop.ToDto();

        Assert.Equal("/images/crops/sunflower.svg", dto.ImageUrl);
        Assert.Equal("Bloom", dto.HarvestOrBloomLabel);
        Assert.Equal("Full Sun", dto.SunRequirement);
        Assert.Equal("30 cm", dto.Spacing);
        Assert.Equal(2, dto.CareSteps.Count);
        Assert.Equal("Tall annuals.", dto.Description);
    }

    [Fact]
    public void CropToDto_UsesPlaceholderWhenImageMissing()
    {
        var crop = new Crop { Name = "Mystery", ImageUrl = "" };
        Assert.Equal("/images/crops/placeholder.svg", crop.ToDto().ImageUrl);
        Assert.Equal("Harvest", crop.ToDto().HarvestOrBloomLabel);
    }
}
