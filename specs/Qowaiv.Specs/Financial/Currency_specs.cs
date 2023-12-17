namespace Financial.Currency_specs;

public class Exists
{
    [Test]
    public void for_currency_exiting_on_date()
        => Currency.EUR.ExistsOnDate(new Date(2003, 01, 01)).Should().BeTrue();

    [Test]
    public void not_for_currency_not_exiting_on_date()
        => Currency.EUR.ExistsOnDate(new Date(1992, 12, 31)).Should().BeFalse();

#if NET6_0_OR_GREATER
    [Test]
    public void for_currency_exiting_on_date_only()
        => Currency.EUR.ExistsOnDate(new DateOnly(2003, 01, 01)).Should().BeTrue();

    [Test]
    public void not_for_currency_not_exiting_on_date_only()
        => Currency.EUR.ExistsOnDate(new Date(1992, 12, 31)).Should().BeFalse();
#endif
}

public class Get_countries
{
    [Test]
    public void on_date()
        => Currency.EUR.GetCountries(new Date(2003, 01, 01)).Should().HaveCount(25);

#if NET6_0_OR_GREATER
    [Test]
    public void on_date_only()
        => Currency.EUR.GetCountries(new DateOnly(2003, 01, 01)).Should().HaveCount(25);
#endif
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Currency.CompareTo(Nil.Object).Should().Be(1);
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Currency).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.FromNull<string>().To<Currency>().Should().Be(Currency.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<Currency>().Should().Be(Currency.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("EUR").To<Currency>().Should().Be(Svo.Currency);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.Currency).Should().Be("EUR");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase(null, null)]
    [TestCase(978L, "EUR")]
    [TestCase(978d, "EUR")]
    [TestCase("EUR", "EUR")]
    public void System_Text_JSON_deserialization(object json, Currency svo)
        => JsonTester.Read_System_Text_JSON<Currency>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("EUR", "EUR")]
    public void System_Text_JSON_serialization(Currency svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase(978L, "EUR")]
    [TestCase("EUR", "EUR")]
    public void convention_based_deserialization(object json, Currency svo)
       => JsonTester.Read<Currency>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("EUR", "EUR")]
    public void convention_based_serialization(Currency svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<Currency>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}
