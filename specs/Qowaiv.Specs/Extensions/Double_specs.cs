using Qowaiv.Extensions;

namespace Extensions.Double_specs;

public class Rounds
{
    [TestCase(1.2345, "1.234")]
    [TestCase(-1.2345, "-1.234")]
    public void Floating_points(double dbl, decimal dec)
        => dbl.Round(3, DecimalRounding.ToEven).Should().Be(dec);
}
