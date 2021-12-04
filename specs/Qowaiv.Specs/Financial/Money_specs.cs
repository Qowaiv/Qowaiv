namespace Financial.Money_specs;

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
       => typeof(Money).Should().HaveTypeConverterDefined();

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("€42.17").To<Money>().Should().Be(Svo.Money);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.Money).Should().Be("€42.17");
        }
    }
}

public class Has_operators
{
    [Test]
    public void to_divide_money_by_money_with_same_currency_as_decimal()
    {
        var ratio = Svo.Money / (16 + Currency.EUR);
        ratio.Should().Be(2.635625m);
    }

    [Test]
    public void to_divide_money_by_money_with_diffrent_currencies_is_not_supported()
    {
        Func<decimal> division =  () => Svo.Money / (16 + Currency.USD);
        division.Should().Throw<CurrencyMismatchException>();
    }
}
