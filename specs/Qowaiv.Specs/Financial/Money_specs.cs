namespace Financial.Money_specs;

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Money.CompareTo(Nil.Object).Should().Be(1);
}

public class Can_be_parsed
{
    [TestCase("€ 42.17")]
    [TestCase("€42.17")]
    [TestCase("€+42.17")]
    [TestCase("EUR42.17")]
    [TestCase("EUR+42.17")]
    [TestCase("EUR 42.17")]
    public void with_currency_before(string before)
        => Money.Parse(before, CultureInfo.InvariantCulture).Should().Be(Svo.Money);

    [TestCase("42.17 €")]
    [TestCase("42.17€")]
    [TestCase("42.17 EUR")]
    [TestCase("42.17EUR")]
    public void with_currency_after(string after)
        => Money.Parse(after, CultureInfo.InvariantCulture).Should().Be(Svo.Money);

    [Test]
    public void with_out_currency()
        => Money.Parse("42.17", CultureInfo.InvariantCulture).Should().Be(42.17 + Currency.Empty);
}

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

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase("EUR 42.17", "EUR 42.17")]
    [TestCase("EUR42.17", "EUR 42.17")]
    [TestCase("€42.17", "EUR 42.17")]
    [TestCase(100L, "100.00")]
    [TestCase(42.17, "42.17")]
    public void System_Text_JSON_deserialization(object json, Money svo)
        => JsonTester.Read_System_Text_JSON<Money>(json).Should().Be(svo);

    [TestCase("EUR42.17", "EUR42.17")]
    public void System_Text_JSON_serialization(Money svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("EUR 42.17", "EUR 42.17")]
    [TestCase("EUR42.17", "EUR 42.17")]
    [TestCase("€42.17", "EUR 42.17")]
    [TestCase(100L, "100.00")]
    [TestCase(42.17, "42.17")]
    public void convention_based_deserialization(object json, Money svo)
        => JsonTester.Read<Money>(json).Should().Be(svo);
    
    [TestCase("EUR42.17", "EUR42.17")]
    public void convention_based_serialization(Money svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        var exception = Assert.Catch(() => JsonTester.Read<Money>(json));
        Assert.IsInstanceOf(exceptionType, exception);
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
    public void to_divide_money_by_money_with_different_currencies_is_not_supported()
    {
        Func<decimal> division =  () => Svo.Money / (16 + Currency.USD);
        division.Should().Throw<CurrencyMismatchException>();
    }
}
