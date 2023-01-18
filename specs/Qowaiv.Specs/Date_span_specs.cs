namespace Date_span_specs;

public class Is_valid
{
    [TestCase("23Y+0M+0D", "Without starting sign")]
    [TestCase("+9998Y+0M+0D", "A lot of years")]
    [TestCase("-9998Y+0M+0D", "A lot of years")]
    [TestCase("0Y+100000M+1D", "A lot of months")]
    [TestCase("0Y-100000M+1D", "A lot of months")]
    [TestCase("0Y+0M+3650000D", "A lot of days")]
    [TestCase("0Y+0M-3650000D", "A lot of days")]
    public void For(string str, string because)
        => DateSpan.TryParse(str).Should().NotBeNull(because);
}
public class Is_invalid
{
    [TestCase("+9999Y+0M+0D", "Years out of reach")]
    [TestCase("-9999Y+0M+0D", "Years out of reach")]
    [TestCase("0Y+0M+4650000D", "Days out of reach")]
    [TestCase("0Y+0M-4650000D", "Days out of reach")]
    public void For(string str, string because)
        => DateSpan.TryParse(str).Should().BeNull(because);
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(DateSpan).Should().HaveTypeConverterDefined();

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("10Y+3M-5D").To<DateSpan>().Should().Be(Svo.DateSpan);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.DateSpan).Should().Be("10Y+3M-5D");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase(null, "0Y+0M+0D")]
    [TestCase(0L, "0Y+0M+0D")]
    [TestCase("0Y+0M+0D", "0Y+0M+0D")]
    [TestCase("1Y+8M+3D", "1Y+8M+3D")]
    public void System_Text_JSON_deserialization(object json, DateSpan svo)
        => JsonTester.Read_System_Text_JSON<DateSpan>(json).Should().Be(svo);

    [TestCase("1Y+8M+3D", "1Y+8M+3D")]
    public void System_Text_JSON_serialization(DateSpan svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase(0L, "0Y+0M+0D")]
    [TestCase("0Y+0M+0D", "0Y+0M+0D")]
    [TestCase("1Y+8M+3D", "1Y+8M+3D")]
    public void convention_based_deserialization(object json, DateSpan svo)
        => JsonTester.Read<DateSpan>(json).Should().Be(svo);

    [TestCase("1Y+8M+3D", "1Y+8M+3D")]
    public void convention_based_serialization(DateSpan svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        var exception = Assert.Catch(() => JsonTester.Read<DateSpan>(json));
        Assert.IsInstanceOf(exceptionType, exception);
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(DateSpan))
       .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
           dataType: typeof(DateSpan),
           description: "Date span, specified in years, months and days.",
           example: "1Y+10M+16D",
           type: "string",
           format: "date-span",
           pattern: @"[+-]?[0-9]+Y[+-][0-9]+M[+-][0-9]+D"));
}
