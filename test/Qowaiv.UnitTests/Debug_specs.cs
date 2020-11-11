using NUnit.Framework;
using Qowaiv.Formatting;
using Qowaiv.Globalization;
using Qowaiv.Json;
using Qowaiv.TestTools;
using Qowaiv.Text;
using System;
using System.Globalization;

namespace Debug_specs
{
    public class Type_is
    {
        [TestCase(typeof(CultureInfoScope))]
        [TestCase(typeof(FormattingArguments))]
        [TestCase(typeof(PostalCodeCountryInfo))]
        [TestCase(typeof(WildcardPattern))]
        public void decorated_with_DebuggerDisplay_attribute(Type svoType)
        {
            DebuggerDisplayAssert.HasAttribute(svoType);
        }
    }

    public class Debugger_displays
    {
        [Test]
        public void Open_API_data()
        {
            var attribute = new OpenApiDataTypeAttribute(
                description: "Year",
                type: "integer",
                format: "0000",
                nullable: true,
                pattern: "^[0-9]{4}$");

            DebuggerDisplayAssert.HasResult("{ type: integer, desc: Year, format: 0000, pattern: ^[0-9]{4}$, nullable: true }", attribute);

        }

        [Test]
        public void empty_postal_code_country_data_for_empty_country()
        {
            DebuggerDisplayAssert.HasResult("Postal code[], none", PostalCodeCountryInfo.GetInstance(Country.Empty));
        }

        [Test]
        public void postal_code_pattern_and_county_for_county_with_postal_codes()
        {
            DebuggerDisplayAssert.HasResult("Postal code[BE], Pattern: ^[1-9][0-9]{3}$", PostalCodeCountryInfo.GetInstance(Country.BE));
        }

        [Test]
        public void postal_code_value_and_county_for_county_with_one_postal_code()
        {
            DebuggerDisplayAssert.HasResult("Postal code[VA], Value: 00120", PostalCodeCountryInfo.GetInstance(Country.VA));
        }

        [Test]
        public void current_and_previous_cultures_for_culture_info_scope()
        {
            using (new CultureInfoScope("en-NL", "en-US"))
            {
                using var scope = new CultureInfoScope("es-ES", "fr-FR");
                DebuggerDisplayAssert.HasResult("CultureInfoScope: [es-ES/fr-FR], Previous: [en-NL/en-US]", scope);
            }
        }
  
        [Test]
        public void formatting_arguments_without_pattern_if_not_specified()
        {
            DebuggerDisplayAssert.HasResult("FormattingArgumentsCollection: 'en-GB', Items: 0", new FormattingArgumentsCollection(new CultureInfo("en-GB")));
        }


        [Test]
        public void formatting_arguments_with_pattern_if_specified()
        {
            DebuggerDisplayAssert.HasResult("Format: 'yyyy-MM-dd', Provider: en-GB", new FormattingArguments("yyyy-MM-dd", new CultureInfo("en-GB")));
        }

        [Test]
        public void wildcard_pattern()
        {
            var pattern = new WildcardPattern("t?st*");
            DebuggerDisplayAssert.HasResult("{t?st*}", pattern);
        }

        [Test]
        public void wildcard_pattern_with_options_if_not_default()
        {
            var pattern = new WildcardPattern("t?st*", WildcardPatternOptions.SingleOrTrailing, StringComparison.Ordinal);
            DebuggerDisplayAssert.HasResult("{t?st*}, SingleOrTrailing, Ordinal", pattern);
        }
    }
}
