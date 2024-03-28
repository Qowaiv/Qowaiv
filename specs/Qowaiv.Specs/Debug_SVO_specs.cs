﻿namespace Debug_SVO_specs;

public class All : SingleValueObjectSpecs
{
    [TestCaseSource(nameof(AllSvos))]
    public void are_decorated_with_DebuggerDisplay_attribute(Type svoType)
        => svoType.Should().BeDecoratedWith<DebuggerDisplayAttribute>();
}

public class Default_value : SingleValueObjectSpecs
{
    [TestCaseSource(nameof(SvosWithEmpty))]
    public void displays_empty(Type svoType)
    {
        var empty = Activator.CreateInstance(svoType);
        empty.Should().HaveDebuggerDisplay("{empty}");
    }

    [TestCase(typeof(Amount), "¤0.00")]
    [TestCase(typeof(Date), "0001-01-01")]
    [TestCase(typeof(DateSpan), "0Y+0M+0D")]
    [TestCase(typeof(Elo), 0d)]
    [TestCase(typeof(Fraction), "⁰⁄₁ = 0")]
    [TestCase(typeof(LocalDateTime), "0001-01-01 12:00:00")]
    [TestCase(typeof(MonthSpan), "0Y+0M")]
    [TestCase(typeof(Money), "0")]
    [TestCase(typeof(Percentage), "0.00%")]
    [TestCase(typeof(StreamSize), "0 byte")]
    [TestCase(typeof(WeekDate), "0001-W01-1")]
    [TestCase(typeof(CustomGuid), "{empty} (ForGuid)")]
    [TestCase(typeof(CustomUuid), "{empty} (ForUuid)")]
    [TestCase(typeof(Int32Id), "{empty} (ForInt32)")]
    [TestCase(typeof(Int64Id), "{empty} (ForInt64)")]
    [TestCase(typeof(StringId), "{empty} (ForString)")]
    public void display(Type svoType, object display)
    {
        var empty = Activator.CreateInstance(svoType);
        empty.Should().HaveDebuggerDisplay(display);
    }
}

public class Unknown_value : SingleValueObjectSpecs
{
    public static IEnumerable<Type> SvosWithDefaultUnknown
        => SvosWithUnknown.Except([typeof(Sex), typeof(InternetMediaType)]);

    [TestCaseSource(nameof(SvosWithDefaultUnknown))]
    public void displays_unknown_for(Type svoType)
    {
        var unknown = Unknown.Value(svoType);
        unknown.Should().HaveDebuggerDisplay("{unknown}");
    }
}
public class Displays
{
    [TestCase(typeof(Amount), "42.17", "¤42.17")]
    [TestCase(typeof(BusinessIdentifierCode), "AEGONL2UXXX", "AEGONL2UXXX")]
    [TestCase(typeof(Country), "VA", "Holy See (VA/VAT)")]
    [TestCase(typeof(CryptographicSeed), "Qowaig==", "Qowaig==")]
    [TestCase(typeof(Currency), "EUR", "Euro (EUR/978)")]
    [TestCase(typeof(Date), "2017-06-10", "2017-06-10")]
    [TestCase(typeof(DateSpan), "10Y+3M-5D", "10Y+3M-5D")]
    [TestCase(typeof(Elo), "1600", 1600d)]
    [TestCase(typeof(EmailAddress), "svo@qowaiv.org", "svo@qowaiv.org")]
    [TestCase(typeof(Fraction), "-69/17", "-⁶⁹⁄₁₇ = -4.05882353")]
    [TestCase(typeof(HouseNumber), "123456789", "123456789")]
    [TestCase(typeof(InternationalBankAccountNumber), "NL20INGB0001234567", "NL20 INGB 0001 2345 67")]
    [TestCase(typeof(InternetMediaType), "application/x-chess-pgn", "application/x-chess-pgn")]
    [TestCase(typeof(LocalDateTime), "1988-06-13 10:10:05.001", "1988-06-13 10:10:05.001")]
    [TestCase(typeof(Money), "EUR 42.17", "€42.17")]
    [TestCase(typeof(Month), "feb", "February (02)")]
    [TestCase(typeof(MonthSpan), "10Y+3M", "10Y+3M")]
    [TestCase(typeof(Percentage), "17.15", "17.15%")]
    [TestCase(typeof(PostalCode), "H0H0H0", "H0H0H0")]
    [TestCase(typeof(StreamSize), "123.5 Megabyte", "123.5 Megabyte")]
    [TestCase(typeof(Year), "2017", "2017")]
    [TestCase(typeof(Uuid), "Qowaiv_SVOLibrary_GUIA", "Qowaiv_SVOLibrary_GUIA")]
    [TestCase(typeof(WeekDate), "1997-W14-6", "1997-W14-6")]
    [TestCase(typeof(YesNo), "Y", "yes")]
    [TestCase(typeof(CustomGuid), "702e186f-f026-4f47-ae4d-fd5f16751f32", "702e186f-f026-4f47-ae4d-fd5f16751f32 (ForGuid)")]
    [TestCase(typeof(Int32Id), "123", "PREFIX123 (ForInt32)")]
    [TestCase(typeof(Int64Id), "123456789", "PREFIX123456789 (ForInt64)")]
    [TestCase(typeof(StringId), "QOWAIV-ID", "QOWAIV-ID (ForString)")]
    public void invariant_representation(Type svoType, string value, object debuggerDisplay)
    {
        var converter = TypeDescriptor.GetConverter(svoType);
        var svo = converter.ConvertFromString(null, CultureInfo.InvariantCulture, value);
        svo.Should().HaveDebuggerDisplay(debuggerDisplay);
    }
}

