namespace Formatting.String_interpolation_specs;

[SetCulture("en-US")]
public class Formats
{
    [Test]
    public void Amount()
    {
        var formatted = 14.17.Amount().ToString("C");
        var interpolated = $"{14.17.Amount():C}";
        interpolated.Should().Be(formatted);
    }

    [Test]
    public void Percentage()
    {
        var formatted = 14.17.Percent().ToString();
        var interpolated = $"{14.17.Percent()}";
        interpolated.Should().Be(formatted);
    }

    [Test]
    public void Fraction()
    {
        var fraction = 4.DividedBy(17);
        var formatted = fraction.ToString();
        var interpolated = $"{fraction}";
        interpolated.Should().Be(formatted);
    }
}
