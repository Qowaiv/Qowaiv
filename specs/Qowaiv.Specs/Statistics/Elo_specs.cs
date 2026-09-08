namespace Statistics.Elo_specs;

public class Z_score
{
    [Test]
    public void calculates()
    {
        var z = Elo.GetZScore(1600, 1500);
        z.Should().BeApproximately(0.64, 0.01);
    }
}

public class Is_invalid
{
    [Test]
    public void for_empty_string() => Elo.TryParse(string.Empty).Should().BeNull();

    [Test]
    public void for_null() => Elo.TryParse(Nil.String).Should().BeNull();

    [Test]
    public void for_garbage() => Elo.TryParse("Not an Elo").Should().BeNull();
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Elo.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Elo.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Elo.Equals(Elo.MinValue).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Elo.Equals(Elo.Create(1732.4)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Elo.Create(1732.4) == Svo.Elo).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Elo.Create(1732.4) == Elo.MinValue).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Elo.Create(1732.4) != Svo.Elo).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Elo.Create(1732.4) != Elo.MinValue).Should().BeTrue();

    [TestCase(0.0, 0)]
    [TestCase(1732.4, -22135344)]
    public void hash_code_is_value_based(Elo svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}

public class Can_be_parsed
{
    [Test]
    public void from_elo_string()
        => Elo.Parse("1400", CultureInfo.InvariantCulture).Should().Be(Elo.Create(1400));

    [TestCase("en-GB", "1732.4")]
    [TestCase("es-EC", "1732,4")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            Elo.Parse(input).Should().Be(Svo.Elo);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            "invalid input".Invoking(Elo.Parse)
                .Should().Throw<FormatException>()
                .WithMessage("Not a valid Elo");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => Elo.TryParse("invalid input", out _).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Elo.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Elo.TryParse("1732.4").Should().Be(Svo.Elo);
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
            Svo.Elo.ToString().Should().Be("1732.4");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Elo.ToString(default(string)).Should().Be(Svo.Elo.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Elo.ToString(string.Empty).Should().Be(Svo.Elo.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_zero()
    {
        using (TestCultures.en_GB.Scoped())
        {
            default(Elo).ToString().Should().Be("0");
        }
    }

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.Elo.ToString(FormatProvider.Empty).Should().Be("1732,4");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Elo.ToString("00000", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '01732', format: '00000'");
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
            Elo.Parse(input).ToString(format).Should().Be(formatted);
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.Elo.ToString(provider: null).Should().Be("1732,4");
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Elo.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_Elo_as_object()
    {
        object obj = Svo.Elo;
        Svo.Elo.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Elo_only()
        => new object().Invoking(Svo.Elo.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new Elo[]
        {
            Elo.Zero,
            Elo.Zero,
            1601,
            2371,
            2416,
            2601,
        };

        var list = new List<Elo> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();

        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        Elo smaller = 17;
        Elo bigger = 19;

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        Elo left = 17;
        Elo right = 17;

        (left < right).Should().BeFalse();
        (left <= right).Should().BeTrue();
        (left > right).Should().BeFalse();
        (left >= right).Should().BeTrue();
    }
}

public class Is_Finite_only
{
    [TestCase(double.NaN)]
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NegativeInfinity)]
    public void Can_not_create(double dbl)
        => dbl.Invoking(Elo.Create)
        .Should().Throw<ArgumentException>()
        .WithMessage("The number can not represent an Elo.*");

    [TestCase(double.NaN)]
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NegativeInfinity)]
    public void Can_not_parse(double dbl)
        => dbl.Invoking(dbl => Elo.Parse(dbl.ToString(CultureInfo.InvariantCulture)))
        .Should().Throw<FormatException>();
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
       => typeof(Elo).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<Elo>().Should().Be(Elo.Zero);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("1732.4").To<Elo>().Should().Be(Svo.Elo);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Elo).Should().Be("1732.4");
        }
    }

    [Test]
    public void from_decimal()
        => Converting.From(1732.4m).To<Elo>().Should().Be(Svo.Elo);

    [Test]
    public void from_double()
        => Converting.From(1732.4).To<Elo>().Should().Be(Svo.Elo);

    [Test]
    public void to_decimal()
        => Converting.To<decimal>().From(Svo.Elo).Should().Be(1732.4m);

    [Test]
    public void to_double()
        => Converting.To<double>().From(Svo.Elo).Should().Be(1732.4);

    [TestCase("0", 0)]
    [TestCase("1732.4", -22135344)]
    public void hash_code_is_value_based(Elo svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase("1600*", 1600.0)]
    [TestCase("1700", 1700.0)]
    [TestCase(1234L, 1234.0)]
    [TestCase(1258.9, 1258.9)]
    public void System_Text_JSON_deserialization(object json, Elo svo)
        => JsonTester.Read_System_Text_JSON<Elo>(json).Should().Be(svo);

    [TestCase(1258.9, 1258.9)]
    public void System_Text_JSON_serialization(Elo svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("1600*", 1600.0)]
    [TestCase("1700", 1700.0)]
    [TestCase(1234L, 1234.0)]
    [TestCase(1258.9, 1258.9)]
    public void convention_based_deserialization(object json, Elo svo)
       => JsonTester.Read<Elo>(json).Should().Be(svo);

    [TestCase(1258.9, 1258.9)]
    public void convention_based_serialization(Elo svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase(double.NaN, typeof(ArgumentOutOfRangeException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<Elo>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}
public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => OpenApiDataType.FromType(typeof(Elo))
        .Should().Be(new OpenApiDataType(
            dataType: typeof(Elo),
            description: "Elo rating system notation.",
            example: 1600d,
            type: "number",
            format: "elo"));
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.Elo);
        xml.Should().Be("1732.4");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<Elo>("1732.4");
        svo.Should().Be(Svo.Elo);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.Elo);
        Svo.Elo.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Elo);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Elo;
        obj.GetSchema().Should().BeNull();
    }
}
