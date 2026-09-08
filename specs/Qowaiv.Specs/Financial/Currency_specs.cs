namespace Financial.Currency_specs;

public class Has_constant
{
    [Test]
    public void Empty_equals_default() => Currency.Empty.Should().Be(default);
}

public class With_domain_logic
{
    [TestCase(true, "")]
    [TestCase(false, "?")]
    [TestCase(false, "EUR")]
    public void IsEmpty_returns(bool result, Currency svo) => svo.IsEmpty().Should().Be(result);

    [TestCase(false, "")]
    [TestCase(true, "?")]
    [TestCase(false, "EUR")]
    public void IsUnknown_returns(bool result, Currency svo) => svo.IsUnknown().Should().Be(result);

    [TestCase(true, "")]
    [TestCase(true, "?")]
    [TestCase(false, "EUR")]
    public void IsEmptyOrUnknown_returns(bool result, Currency svo) => svo.IsEmptyOrUnknown().Should().Be(result);
}

public class Current
{
    [Test]
    public void for_current_culture_nl_NL_is_EUR()
    {
        using (TestCultures.nl_NL.Scoped())
        {
            Currency.Current.Should().Be(Currency.EUR);
        }
    }

    [Test]
    public void for_current_culture_es_EC_is_USD()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Currency.Current.Should().Be(Currency.USD);
        }
    }

    [Test]
    public void for_current_culture_en_is_empty()
    {
        using (TestCultures.en.Scoped())
        {
            Currency.Current.Should().Be(Currency.Empty);
        }
    }
}

public class Has_properties
{
    [Test]
    public void Symbol_for_AZN() => Currency.AZN.Symbol.Should().Be("₼");
}

public class Exists
{
    [Test]
    public void for_currency_exiting_on_Date()
        => Currency.EUR.ExistsOnDate(new Date(2003, 01, 01)).Should().BeTrue();

    [Test]
    public void not_for_currency_not_exiting_on_Date()
        => Currency.EUR.ExistsOnDate(new Date(1992, 12, 31)).Should().BeFalse();

#if NET8_0_OR_GREATER
    [Test]
    public void for_currency_exiting_on_DateOnly()
        => Currency.EUR.ExistsOnDate(new DateOnly(2003, 01, 01)).Should().BeTrue();

    [Test]
    public void not_for_currency_not_exiting_on_DateOnly()
        => Currency.EUR.ExistsOnDate(new Date(1992, 12, 31)).Should().BeFalse();
#endif
}

public class Get_countries
{
    [Test]
    public void on_Date()
        => Currency.EUR.GetCountries(new Date(2003, 01, 01)).Should().HaveCount(25);

#if NET8_0_OR_GREATER
    [Test]
    public void on_DateOnly()
        => Currency.EUR.GetCountries(new DateOnly(2003, 01, 01)).Should().HaveCount(25);
#endif
}

public class Can_be_parsed
{
    [Test]
    public void from_null_string_represents_Empty()
        => Currency.Parse(null).Should().Be(Currency.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => Currency.Parse(string.Empty).Should().Be(Currency.Empty);

    [Test]
    public void from_question_mark_represents_Unknown()
        => Currency.Parse("?").Should().Be(Currency.Unknown);

    [Test]
    public void from_generic_currency_symbol_represents_Unknown()
        => Currency.Parse("¤").Should().Be(Currency.Unknown);

    [Test]
    public void from_euro_symbol()
        => Currency.Parse("€").Should().Be(Currency.EUR);

    [TestCase("en-GB", "EUR")]
    [TestCase("es-EC", "EUR")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            Currency.Parse(input).Should().Be(Svo.Currency);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            "invalid input".Invoking(Currency.Parse)
                .Should().Throw<FormatException>()
                .WithMessage("Not a valid currency");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => Currency.TryParse("invalid input", out _).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Currency.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Currency.TryParse(Svo.Currency.ToString()).Should().Be(Svo.Currency);
        }
    }
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Currency.ToString().Should().Be("EUR");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Currency.ToString(default(string)).Should().Be(Svo.Currency.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Currency.ToString(string.Empty).Should().Be(Svo.Currency.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(Currency).ToString().Should().BeEmpty();

    [Test]
    public void unknown_value_is_represented_as_unknown()
        => Currency.Unknown.ToString().Should().Be("?");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.Currency.ToString(FormatProvider.Empty).Should().Be("EUR");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Currency.ToString("$: e", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '€: Euro', format: '$: e'");
    }

    [Test]
    public void with_dutch_translation_using_default_format()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            Currency.Parse("Amerikaanse dollar").ToString().Should().Be("USD");
        }
    }

    [Test]
    public void with_english_name_using_full_name_format()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Currency.Parse("pound sterling").ToString("f").Should().Be("Pound sterling");
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.Currency.ToString(provider: null).Should().Be("EUR");
        }
    }

    [Test]
    public void amount_with_BYR()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var number = (Amount)1200.34m;
            number.ToString("C", Currency.BYR).Should().Be("BYR1,200");
        }
    }

    [Test]
    public void amount_with_ANG()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var number = (Amount)12.34m;
            number.ToString("C", Currency.ANG).Should().Be("NAf.12.34");
        }
    }

    [Test]
    public void decimal_with_ANG()
    {
        using (TestCultures.en_GB.Scoped())
        {
            12.34m.ToString("C", Currency.ANG).Should().Be("NAf.12.34");
        }
    }

    [Test]
    public void decimal_with_TND()
    {
        using (TestCultures.en_GB.Scoped())
        {
            12.34m.ToString("C", Currency.TND).Should().Be("TND12.340");
        }
    }

    [Test]
    public void decimal_and_double_with_EUR()
    {
        using (TestCultures.en_GB.Scoped())
        {
            12.34m.ToString("C", Currency.EUR).Should().Be("€12.34");
            12.34.ToString("C", Currency.EUR).Should().Be("€12.34");
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Currency.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_Currency_as_object()
    {
        object obj = Svo.Currency;
        Svo.Currency.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Currency_only()
        => new object().Invoking(Svo.Currency.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        Currency[] sorted =
        [
            Currency.Empty,
            Currency.Empty,
            Currency.AED,
            Currency.BAM,
            Currency.CAD,
            Currency.EUR,
        ];

        var list = new List<Currency> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();

        list.Should().BeEquivalentTo(sorted);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Currency).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<Currency>().Should().Be(Currency.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<Currency>().Should().Be(Currency.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("EUR").To<Currency>().Should().Be(Svo.Currency);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Currency).Should().Be("EUR");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
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

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Currency.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Currency.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Currency.Equals(Currency.Empty).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Currency.Equals(Currency.Parse("eur", CultureInfo.InvariantCulture)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Currency.Parse("eur", CultureInfo.InvariantCulture) == Svo.Currency).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Currency.Parse("eur", CultureInfo.InvariantCulture) == Currency.Empty).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Currency.Parse("eur", CultureInfo.InvariantCulture) != Svo.Currency).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Currency.Parse("eur", CultureInfo.InvariantCulture) != Currency.Empty).Should().BeTrue();

    [Test]
    public void formatted_and_unformatted_are_equal()
    {
        var l = Currency.Parse("eur", CultureInfo.InvariantCulture);
        var r = Currency.Parse("EUR", CultureInfo.InvariantCulture);
        l.Equals(r).Should().BeTrue();
    }

    [TestCase("", 0)]
    [TestCase("EUR", -943833935)]
    public void hash_code_is_value_based(Currency svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.Currency);
        xml.Should().Be("EUR");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<Currency>("EUR");
        svo.Should().Be(Svo.Currency);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.Currency);
        Svo.Currency.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Currency);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Currency;
        obj.GetSchema().Should().BeNull();
    }
}
