namespace Debug_specs;

public class Type_is
{
    [TestCase(typeof(CultureInfoScope))]
    [TestCase(typeof(FormattingArguments))]
    [TestCase(typeof(PostalCodeCountryInfo))]
    [TestCase(typeof(WildcardPattern))]
    public void decorated_with_DebuggerDisplay_attribute(Type svoType)
    => svoType.Should().BeDecoratedWith<DebuggerDisplayAttribute>();
}

public class Debugger_displays
{
    [Test]
    public void empty_postal_code_country_data_for_empty_country()
        => PostalCodeCountryInfo.GetInstance(Country.Empty)
        .Should().HaveDebuggerDisplay("Postal code[], none");

    [Test]
    public void postal_code_pattern_and_county_for_county_with_postal_codes()
        => PostalCodeCountryInfo.GetInstance(Country.BE)
        .Should().HaveDebuggerDisplay("Postal code[BE], Pattern: ^[1-9][0-9]{3}$");

    [Test]
    public void postal_code_value_and_county_for_county_with_one_postal_code()
        => PostalCodeCountryInfo.GetInstance(Country.VA)
        .Should().HaveDebuggerDisplay("Postal code[VA], Value: 00120");

    [Test]
    public void current_and_previous_cultures_for_culture_info_scope()
    {
        using (new CultureInfoScope("en-NL", "en-US"))
        {
            using var scope = new CultureInfoScope("es-ES", "fr-FR");
            scope.Should().HaveDebuggerDisplay("CultureInfoScope: [es-ES/fr-FR], Previous: [en-NL/en-US]");
        }
    }

    [Test]
    public void formatting_arguments_without_pattern_if_not_specified()
        => ((object)new FormattingArgumentsCollection(new CultureInfo("en-GB")))
        .Should().HaveDebuggerDisplay("FormattingArgumentsCollection: 'en-GB', Items: 0");


    [Test]
    public void formatting_arguments_with_pattern_if_specified()
        => new FormattingArguments("yyyy-MM-dd", new CultureInfo("en-GB"))
        .Should().HaveDebuggerDisplay("Format: 'yyyy-MM-dd', Provider: en-GB");

    [Test]
    public void wildcard_pattern()
        => new WildcardPattern("t?st*")
        .Should().HaveDebuggerDisplay("{t?st*}");

    [Test]
    public void wildcard_pattern_with_options_if_not_default()
        => new WildcardPattern("t?st*", WildcardPatternOptions.SingleOrTrailing, StringComparison.Ordinal)
        .Should().HaveDebuggerDisplay("{t?st*}, SingleOrTrailing, Ordinal");
}
