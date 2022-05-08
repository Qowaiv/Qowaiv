﻿namespace DateSpan_specs;

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
