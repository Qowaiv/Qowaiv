namespace Financial.Amount_specs;

public class Has_constant
{
    [Test]
    public void MinValue_equal_to_decimal_MinValue() => Amount.MinValue.Should().Be(decimal.MinValue.Amount());

    [Test]
    public void MaxValue_equal_to_decimal_MaxValue() => Amount.MaxValue.Should().Be(decimal.MaxValue.Amount());
}

public class Can_be_parsed
{
    [Test]
    public void from_amount_string()
        => Amount.Parse("14.1804", CultureInfo.InvariantCulture).Should().Be((Amount)14.1804m);

    [TestCase("en-GB", "42.17")]
    [TestCase("es-EC", "42,17")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            Amount.Parse(input).Should().Be(Svo.Amount);
        }
    }

    [Test]
    public void with_custom_number_format_info()
    {
        var info = new NumberFormatInfo
        {
            CurrencyGroupSeparator = "#",
            CurrencyDecimalSeparator = "*",
        };
        Amount.Parse("5#123*34", info).Should().Be((Amount)5123.34m);
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            "invalid input".Invoking(Amount.Parse)
                .Should().Throw<FormatException>()
                .WithMessage("Not a valid amount");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => Amount.TryParse("invalid input", out _).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Amount.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Amount.TryParse("42.17").Should().Be(Svo.Amount);
        }
    }
}

public class Min_and_max
{
    [TestCase(17, 17)]
    [TestCase(17, 18)]
    [TestCase(18, 17)]
    public void Min_of_two_returns_minimum(Amount a1, Amount a2)
        => Amount.Min(a1, a2).Should().Be(17.Amount());

    [Test]
    public void Min_of_collection_returns_minimum()
        => Amount.Min(700.Amount(), 17.Amount(), 48.Amount()).Should().Be(17.Amount());

    [TestCase(17, 17)]
    [TestCase(17, 16)]
    [TestCase(16, 17)]
    public void Max_of_two_returns_minimum(Amount a1, Amount a2)
        => Amount.Max(a1, a2).Should().Be(17.Amount());

    [Test]
    public void Max_of_collection_returns_maximum()
        => Amount.Max(7.Amount(), 17.Amount(), -48.Amount()).Should().Be(17.Amount());
}

public class Can_not_be_parsed
{
    [TestCase(NumberStyles.Number)]
    [TestCase(NumberStyles.Integer)]
    public void strings_with_currency_if_style_does_not_allow_currency_sign(NumberStyles style)
        => Amount.TryParse("42 EUR", style, CultureInfo.InvariantCulture, out _)
            .Should().BeFalse();

    [TestCase(NumberStyles.HexNumber)]
    [TestCase(NumberStyles.AllowExponent)]
    public void using_a_number_style_other_then_Curency(NumberStyles style)
         => style.Invoking(s => Amount.TryParse("4.50", s, CultureInfo.InvariantCulture, out _))
             .Should().Throw<ArgumentOutOfRangeException>()
             .WithMessage("The number style '*' is not supported.*");
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Amount.ToString().Should().Be("42.17");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Amount.ToString(default(string)).Should().Be(Svo.Amount.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Amount.ToString(string.Empty).Should().Be(Svo.Amount.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_zero()
    {
        using (TestCultures.en_GB.Scoped())
        {
            default(Amount).ToString().Should().Be("0");
        }
    }

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.Amount.ToString(FormatProvider.Empty).Should().Be("42,17");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Amount.ToString("#.0", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '42.2', format: '#.0'");
    }

    [TestCase("nl-BE", null, "1600,1", "1600,1")]
    [TestCase("en-GB", null, "1600.1", "1600.1")]
    [TestCase("nl-BE", "0000", "800", "0800")]
    [TestCase("en-GB", "0000", "800", "0800")]
    [TestCase("es-EC", "00000.0", "1700", "01700,0")]
    public void culture_dependent(CultureInfo culture, string format, string input, string formatted)
    {
        using (culture.Scoped())
        {
            Amount.Parse(input).ToString(format).Should().Be(formatted);
        }
    }

    [Test]
    public void with_currency_format_fr_FR()
    {
        using (TestCultures.fr_FR.Scoped())
        {
            ((Amount)170.42).ToString("C").Should().Be("170,42 €");
        }
    }

    [Test]
    public void with_custom_number_format_info()
    {
        var info = new NumberFormatInfo
        {
            CurrencyGroupSeparator = "#",
            CurrencyDecimalSeparator = "*",
        };
        var formatted = ((Amount)12345678.235m).ToString("#,##0.0000", info);
        formatted.Should().Be("12#345#678*2350");
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.Amount.ToString(provider: null).Should().Be("42,17");
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Amount.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_Amount_as_object()
    {
        object obj = Svo.Amount;
        Svo.Amount.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Amount_only()
        => new object().Invoking(Svo.Amount.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        Amount[] sorted =
        [
            Amount.Zero,
            Amount.Zero,
            0.23.Amount(),
            1.24.Amount(),
            2.27.Amount(),
            1300.Amount(),
        ];

        var list = new List<Amount> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();

        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        var smaller = 17.Amount();
        var bigger = 19.Amount();

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        var left = 17.Amount();
        var right = 17.Amount();

        (left < right).Should().BeFalse();
        (left <= right).Should().BeTrue();
        (left > right).Should().BeFalse();
        (left >= right).Should().BeTrue();
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
       => typeof(Amount).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<Amount>().Should().Be(Amount.Zero);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("42.17").To<Amount>().Should().Be(Svo.Amount);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.To<string>().From(Svo.Amount).Should().Be("42.17");
        }
    }

    [Test]
    public void from_decimal()
        => Converting.From(42.17m).To<Amount>().Should().Be(Svo.Amount);

    [Test]
    public void from_double()
        => Converting.From(42.17).To<Amount>().Should().Be(Svo.Amount);

    [Test]
    public void to_decimal()
        => Converting.To<decimal>().From(Svo.Amount).Should().Be(42.17m);

    [Test]
    public void to_double()
        => Converting.To<double>().From(Svo.Amount).Should().Be(42.17);


    public class Supports_JSON_serialization
    {
#if NET8_0_OR_GREATER
        [TestCase("1234.56", 1234.56)]
        [TestCase(1234.56, 1234.56)]
        [TestCase(1234L, 1234.00)]
        public void System_Text_JSON_deserialization(object json, Amount svo)
            => JsonTester.Read_System_Text_JSON<Amount>(json).Should().Be(svo);

        [Test]
        public void System_Text_JSON_deserialization_min_value()
        {
            var amount = System.Text.Json.JsonSerializer.Deserialize<Amount>("-7.922816251426434E+28");
            amount.Should().Be(Amount.MinValue);
        }

        [TestCase("7.922816251426434E+28")]
        [TestCase("79228162514264337593543950335")]
        public void System_Text_JSON_deserialization_max_value(string json)
        {
            var amount = System.Text.Json.JsonSerializer.Deserialize<Amount>(json);
            amount.Should().Be(Amount.MaxValue);
        }

        [TestCase(1234.56, 1234.56)]
        public void System_Text_JSON_serialization(Amount svo, object json)
            => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);

        [Test]
        public void System_Text_JSON_serialization_Max_value()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(Amount.MaxValue);
            json.Should().Be("79228162514264337593543950335");
        }
#endif
        [TestCase("1234.56", 1234.56)]
        [TestCase(1234.56, 1234.56)]
        [TestCase(1234L, 1234.00)]
        public void convention_based_deserialization(object json, Amount svo)
          => JsonTester.Read<Amount>(json).Should().Be(svo);

        [TestCase(1234.56, 1234.56)]
        public void convention_based_serialization(Amount svo, object json)
            => JsonTester.Write(svo).Should().Be(json);

        [Test]
        public void convention_based_serialization_max_value()
            => JsonTester.Write(Amount.MaxValue).Should().Be(79228162514264337593543950335M);

        [TestCase("Invalid input", typeof(FormatException))]
        [TestCase("2017-06-11", typeof(FormatException))]
        public void throws_for_invalid_json(object json, Type exceptionType)
            => json
                .Invoking(JsonTester.Read<Amount>)
                .Should().Throw<Exception>()
                .And.Should().BeOfType(exceptionType);
    }

    public class Casts_explicit
    {
        public class From
        {
            [Test]
            public void Int32() => ((Amount)42).Should().Be(42.Amount());

            [Test]
            public void Int64() => ((Amount)42L).Should().Be(42.Amount());

            [Test]
            public void Decimal() => ((Amount)42.0m).Should().Be(42.Amount());

            [Test]
            public void Double() => ((Amount)42.0).Should().Be(42.Amount());
        }

        public class To
        {
            [Test]
            public void Int32() => ((int)42.Amount()).Should().Be(42);

            [Test]
            public void Int64() => ((long)42.Amount()).Should().Be(42L);

            [Test]
            public void Decimal() => ((decimal)42.Amount()).Should().Be(42m);

            [Test]
            public void Double() => ((double)42.Amount()).Should().Be(42.0);
        }
    }
    public class Has_operators
    {
        [Test]
        public void to_divide_amount_by_amount_as_decimal()
        {
            var ratio = Svo.Amount / 2.Amount();
            ratio.Should().Be(21.085m);
        }
    }
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.Amount);
        xml.Should().Be("42.17");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<Amount>("42.17");
        svo.Should().Be(Svo.Amount);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.Amount);
        Svo.Amount.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Amount);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Amount;
        obj.GetSchema().Should().BeNull();
    }
}
