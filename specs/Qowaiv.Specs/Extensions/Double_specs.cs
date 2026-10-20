using Qowaiv.Extensions;

namespace Extensions.Double_specs;

public class ToDecimal
{
    [TestCase(1.2345, "1.234")]
    [TestCase(-1.2345, "-1.234")]
    public void Floating_points_with_rounding(double dbl, decimal dec)
        => dbl.ToDecimal(3, DecimalRounding.ToEven).Should().Be(dec);

    [Test]
    public void Floating_points_without_rounding()
#if NET11_0_OR_GREATER
        => 0.333.ToDecimal().Should().Be(0.3330000000000000182076576039m);
#else
        => 0.333.ToDecimal().Should().Be(0.333m);
#endif
}
