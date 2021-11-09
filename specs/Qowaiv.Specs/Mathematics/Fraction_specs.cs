namespace Mathematics.Fraction_specs;

public class Has_humanizer_creators
{
    [Test]
    public void int_DividedBy_int()
        => 12.DividedBy(24).Should().Be(new Fraction(1, 2));

    [Test]
    public void long_DividedBy_long()
       => 12L.DividedBy(24L).Should().Be(new Fraction(1, 2));

    [Test]
    public void Fraction_from_double()
        => 0.5.Fraction().Should().Be(new Fraction(1, 2));

    [Test]
    public void Fraction_from_decimal()
        => 0.5m.Fraction().Should().Be(new Fraction(1, 2));
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Fraction).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From<string>(null).To<Fraction>().Should().Be(Fraction.Zero);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<Fraction>().Should().Be(Fraction.Zero);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("-69/17").To<Fraction>().Should().Be(Svo.Fraction);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.Fraction).Should().Be("-69/17");
        }
    }
}
