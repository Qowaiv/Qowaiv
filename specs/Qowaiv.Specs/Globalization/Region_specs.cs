using System.Resources;

namespace Globalization.Region_specs;

public class Resources_
{
    public Resources_()
    {
        var additional = new ResourceManager("Qowaiv.Specs.Globalization.AdditionalRegions", typeof(Resources_).Assembly);
        Region.RegisterAdditional(additional);
    }

    [Test]
    public void All()
    {
        var regions = Region.All;
        regions.Should().HaveCount(3);
    }
}
