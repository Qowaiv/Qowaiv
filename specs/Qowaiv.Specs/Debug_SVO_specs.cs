namespace Debug_SVO_specs;

public class All : SingleValueObjectSpecs
{
    [Test]
    [MethodDataSource(nameof(AllSvos))]
    public void are_decorated_with_DebuggerDisplay_attribute(Type svoType)
        => svoType.Should().BeDecoratedWith<DebuggerDisplayAttribute>();
}

public class Default_value : SingleValueObjectSpecs
{
    [Test]
    [MethodDataSource(nameof(SvosWithEmpty))]
    public void displays_empty(Type svoType)
    {
        var empty = Activator.CreateInstance(svoType);
        empty.Should().HaveDebuggerDisplay("{empty}");
    }

    [Test]
    [Arguments(typeof(Amount), "¤0.00")]
    [Arguments(typeof(Date), "0001-01-01")]
    [Arguments(typeof(DateSpan), "0Y+0M+0D")]
    [Arguments(typeof(Elo), 0d)]
    [Arguments(typeof(Fraction), "⁰⁄₁ = 0")]
    [Arguments(typeof(LocalDateTime), "0001-01-01 12:00:00")]
    [Arguments(typeof(MonthSpan), "0Y+0M")]
    [Arguments(typeof(Money), "0")]
    [Arguments(typeof(Percentage), "0.00%")]
    [Arguments(typeof(StreamSize), "0 byte")]
    [Arguments(typeof(WeekDate), "0001-W01-1")]
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

    [Test]
    [MethodDataSource(nameof(SvosWithDefaultUnknown))]
    public void displays_unknown_for(Type svoType)
    {
        var unknown = Unknown.Value(svoType);
        unknown.Should().HaveDebuggerDisplay("{unknown}");
    }
}
public class Displays
{
    [Test]
    [Arguments(typeof(Amount), "42.17", "¤42.17")]
    [Arguments(typeof(BusinessIdentifierCode), "AEGONL2UXXX", "AEGONL2UXXX")]
    [Arguments(typeof(Country), "VA", "Holy See (VA/VAT)")]
    [Arguments(typeof(CryptographicSeed), "Qowaig==", "Qowaig==")]
    [Arguments(typeof(Currency), "EUR", "Euro (EUR/978)")]
    [Arguments(typeof(Date), "2017-06-10", "2017-06-10")]
    [Arguments(typeof(DateSpan), "10Y+3M-5D", "10Y+3M-5D")]
    [Arguments(typeof(Elo), "1600", 1600d)]
    [Arguments(typeof(EmailAddress), "svo@qowaiv.org", "svo@qowaiv.org")]
    [Arguments(typeof(Fraction), "-69/17", "-⁶⁹⁄₁₇ = -4.05882353")]
    [Arguments(typeof(HouseNumber), "123456789", "123456789")]
    [Arguments(typeof(InternationalBankAccountNumber), "NL20INGB0001234567", "NL20 INGB 0001 2345 67")]
    [Arguments(typeof(InternetMediaType), "application/x-chess-pgn", "application/x-chess-pgn")]
    [Arguments(typeof(LocalDateTime), "1988-06-13 10:10:05.001", "1988-06-13 10:10:05.001")]
    [Arguments(typeof(Money), "EUR 42.17", "€42.17")]
    [Arguments(typeof(Month), "feb", "February (02)")]
    [Arguments(typeof(MonthSpan), "10Y+3M", "10Y+3M")]
    [Arguments(typeof(Percentage), "17.15", "17.15%")]
    [Arguments(typeof(PostalCode), "H0H0H0", "H0H0H0")]
    [Arguments(typeof(StreamSize), "123.5 Megabyte", "123.5 Megabyte")]
    [Arguments(typeof(Year), "2017", "2017")]
    [Arguments(typeof(Uuid), "Qowaiv_SVOLibrary_GUIA", "Qowaiv_SVOLibrary_GUIA")]
    [Arguments(typeof(WeekDate), "1997-W14-6", "1997-W14-6")]
    [Arguments(typeof(YesNo), "Y", "yes")]
    public void invariant_representation(Type svoType, string value, object debuggerDisplay)
    {
        var converter = TypeDescriptor.GetConverter(svoType);
        var svo = converter.ConvertFromString(null, CultureInfo.InvariantCulture, value);
        svo.Should().HaveDebuggerDisplay(debuggerDisplay);
    }
}

