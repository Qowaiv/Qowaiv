using Qowaiv.Sustainability;

namespace Energy_label_specs;

public class With_domain_logic
{
    [TestCase(true, "A++")]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, EnergyLabel svo) => svo.HasValue.Should().Be(result);

    [TestCase(true, "A++")]
    [TestCase(false, "?")]
    [TestCase(false, "")]
    public void IsKnown_is(bool result, EnergyLabel svo) => svo.IsKnown.Should().Be(result);

    [TestCase(false, "A++")]
    [TestCase(false, "?")]
    [TestCase(true, "")]
    public void IsEmpty_returns(bool result, EnergyLabel svo)
        => svo.IsEmpty().Should().Be(result);

    [TestCase(false, "A++")]
    [TestCase(true, "?")]
    [TestCase(true, "")]
    public void IsEmptyOrUnknown_returns(bool result, EnergyLabel svo)
        => svo.IsEmptyOrUnknown().Should().Be(result);

    [TestCase(false, "A++")]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void IsUnknown_returns(bool result, EnergyLabel svo)
        => svo.IsUnknown().Should().Be(result);

    [TestCase(-1)]
    [TestCase(5)]
    [TestCase(6)]
    public void with_zero_to_four_plusses(int plusses)
        => plusses.Invoking(_ => EnergyLabel.A(plusses))
        .Should().Throw<ArgumentOutOfRangeException>();
}

public class Is_valid_for
{
    [Test]
    public void whitespace()
        => EnergyLabel.TryParse(" ").Should().NotBeNull();

    [Test]
    public void string_empty()
     => EnergyLabel.TryParse(string.Empty).Should().NotBeNull();

    [Test]
    public void string_null()
        => EnergyLabel.TryParse(null).Should().NotBeNull();

    [TestCase("?")]
    [TestCase("unknown")]
    public void strings_representing_unknown(string input)
        => EnergyLabel.TryParse(input).Should().NotBeNull();

    [TestCase("g", "de")]
    [TestCase("G", "de")]
    [TestCase("F", "fr")]
    [TestCase("E", "es")]
    [TestCase("D", "nl")]
    [TestCase("C", "nl")]
    [TestCase("B", "nl")]
    [TestCase("A", "nl")]
    [TestCase("A+", "nl")]
    [TestCase("A++", "nl")]
    [TestCase("A+++", "nl")]
    [TestCase("A++++", "nl")]
    [TestCase("a++++", "pt")]
    public void @string(string input, CultureInfo culture)
        => EnergyLabel.TryParse(input, culture).Should().NotBeNull();
}

public class Is_not_valid_for
{
    [TestCase("H")]
    [TestCase("I")]
    public void H_and_lower(string label)
        => EnergyLabel.TryParse(label).Should().BeNull();

    [TestCase("G+")]
    [TestCase("A+++++")]
    [TestCase("A++++++")]
    public void Plus_overload(string label)
        => EnergyLabel.TryParse(label).Should().BeNull();

    [Test]
    public void garbage()
        => EnergyLabel.TryParse("garbage").Should().BeNull();
}

public class Has_constant
{
    [Test]
    public void Empty() => EnergyLabel.Empty.Should().Be(default);

    [Test]
    public void A() => EnergyLabel.A().Should().Be(EnergyLabel.Parse("A"));

    [Test]
    public void A_plus([Range(1, 4)] int plus) => EnergyLabel.A(plus).Should().Be(EnergyLabel.Parse("A" + new string('+', plus)));

    [Test]
    public void B() => EnergyLabel.B.Should().Be(EnergyLabel.Parse("B"));

    [Test]
    public void C() => EnergyLabel.C.Should().Be(EnergyLabel.Parse("C"));

    [Test]
    public void D() => EnergyLabel.D.Should().Be(EnergyLabel.Parse("D"));

    [Test]
    public void E() => EnergyLabel.E.Should().Be(EnergyLabel.Parse("E"));

    [Test]
    public void F() => EnergyLabel.F.Should().Be(EnergyLabel.Parse("F"));

    [Test]
    public void G() => EnergyLabel.G.Should().Be(EnergyLabel.Parse("G"));

    [Test]
    public void Unknown() => EnergyLabel.Unknown.Should().Be(EnergyLabel.Parse("?"));
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.EnergyLabel.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.EnergyLabel.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.EnergyLabel.Equals(EnergyLabel.G).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.EnergyLabel.Equals(EnergyLabel.A(2)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.EnergyLabel == EnergyLabel.A(2)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.EnergyLabel == EnergyLabel.G).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.EnergyLabel != EnergyLabel.A(2)).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.EnergyLabel != EnergyLabel.G).Should().BeTrue();

    [TestCase("", 0)]
    [TestCase("A++", 665630170)]
    public void hash_code_is_value_based(EnergyLabel svo, int hash)
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
    public void from_null_string_represents_Empty()
        => EnergyLabel.TryParse(null).Should().Be(EnergyLabel.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => EnergyLabel.Parse(string.Empty).Should().Be(EnergyLabel.Empty);

    [Test]
    public void from_question_mark_represents_Unknown()
        => EnergyLabel.Parse("?").Should().Be(EnergyLabel.Unknown);

    [TestCase("en", "a++")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            EnergyLabel.Parse(input).Should().Be(Svo.EnergyLabel);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Func<EnergyLabel> parse = () => EnergyLabel.Parse("invalid input");
            parse.Should().Throw<FormatException>()
                .WithMessage("Not a valid energy label");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => (EnergyLabel.TryParse("invalid input", out _)).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => EnergyLabel.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => EnergyLabel.TryParse("A++").Should().Be(Svo.EnergyLabel);
}

public class Has_custom_formatting
{
    [TestCase("G")]
    [TestCase("F")]
    [TestCase("E")]
    [TestCase("D")]
    [TestCase("C")]
    [TestCase("B")]
    [TestCase("A")]
    [TestCase("A+")]
    [TestCase("A++")]
    [TestCase("A+++")]
    [TestCase("A++++")]
    public void _default(string label)
        => EnergyLabel.Parse(label).ToString().Should().Be(label);

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.EnergyLabel.ToString().Should().Be(Svo.EnergyLabel.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.EnergyLabel.ToString().Should().Be(Svo.EnergyLabel.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(EnergyLabel).ToString().Should().BeEmpty();

    [Test]
    public void unknown_value_is_represented_as_unknown()
        => EnergyLabel.Unknown.ToString().Should().Be("?");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.EnergyLabel.ToString(FormatProvider.Empty).Should().Be("A++");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.EnergyLabel.ToString("SomeFormat", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: 'A++', format: 'SomeFormat'");
    }

    [TestCase(null, "A++", "A++")]
    [TestCase("U", "B", "B")]
    [TestCase("l", "G", "g")]
    [TestCase("l", "A++", "a++")]
    public void format_dependent(string format, EnergyLabel svo, string formatted)
        => svo.ToString(format).Should().Be(formatted);

#if NET8_0_OR_GREATER

    public class Span_formattable
    {
        [Test]
        public void Skips_custom_formatters()
        {
            Span<char> span = stackalloc char[128];
            Svo.EnergyLabel.TryFormat(span, out int charsWritten, default, FormatProvider.CustomFormatter).Should().BeFalse();
            charsWritten.Should().Be(0);
        }

        [Test]
        public void formats_empty() => $"{EnergyLabel.Empty}".Should().BeEmpty();

        [Test]
        public void formats_unknown() => $"{EnergyLabel.Unknown}".Should().Be("?");

        [Test]
        public void formats_known() => $"{Svo.EnergyLabel:l}".Should().Be("a++");

        [Test]
        public void Skips_insufficient_span_sizes()
        {
            Span<char> span = stackalloc char[2];
            Svo.EnergyLabel.TryFormat(span, out int charsWritten, default, TestCultures.nl_NL).Should().BeFalse();
            charsWritten.Should().Be(0);
        }
    }
#endif
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.EnergyLabel.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_EnergyLabel_as_object()
    {
        object obj = Svo.EnergyLabel;
        Svo.EnergyLabel.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_EnergyLabel_only()
        => new object().Invoking(Svo.EnergyLabel.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            default,
            default,
            EnergyLabel.F,
            EnergyLabel.C,
            EnergyLabel.A(),
            EnergyLabel.Unknown,
        };

        var list = new List<EnergyLabel> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        var smaller = EnergyLabel.B;
        var bigger = EnergyLabel.A(2);

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        var left = EnergyLabel.A(2);
        var right = EnergyLabel.A(2);

        (left < right).Should().BeFalse();
        (left <= right).Should().BeTrue();
        (left > right).Should().BeFalse();
        (left >= right).Should().BeTrue();
    }

    [TestCase("?", "A++")]
    [TestCase("?", "")]
    [TestCase("A++", "?")]
    [TestCase("", "?")]
    [TestCase("?", "?")]
    public void by_operators_unknown_always_false(EnergyLabel l, EnergyLabel r)
    {
        (l < r).Should().BeFalse();
        (l <= r).Should().BeFalse();
        (l > r).Should().BeFalse();
        (l >= r).Should().BeFalse();
    }
}

public class Casts
{
    [Test]
    public void explicitly_from_string()
    {
        var casted = (EnergyLabel)"A++";
        casted.Should().Be(Svo.EnergyLabel);
    }

    [Test]
    public void explicitly_to_string()
    {
        var casted = (string)Svo.EnergyLabel;
        casted.Should().Be("A++");
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(EnergyLabel).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<EnergyLabel>().Should().Be(EnergyLabel.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<EnergyLabel>().Should().Be(EnergyLabel.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("A++").To<EnergyLabel>().Should().Be(Svo.EnergyLabel);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.EnergyLabel).Should().Be("A++");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase("?", "?")]
    [TestCase("C", "C")]
    [TestCase("A++", "A++")]
    public void System_Text_JSON_deserialization(object json, EnergyLabel svo)
        => JsonTester.Read_System_Text_JSON<EnergyLabel>(json).Should().Be(svo);

    [TestCase("?", "?")]
    [TestCase("C", "C")]
    [TestCase("A++", "A++")]
    public void System_Text_JSON_serialization(EnergyLabel svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("?", "unknown")]
    [TestCase("C", "C")]
    [TestCase("A++", "A++")]
    public void convention_based_deserialization(EnergyLabel svo, object json)
        => JsonTester.Read<EnergyLabel>(json).Should().Be(svo);

    [TestCase(null, "")]
    [TestCase("C", "C")]
    [TestCase("A++", "A++")]
    public void convention_based_serialization(object json, EnergyLabel svo)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(5L, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        Func<EnergyLabel> read = () => JsonTester.Read<EnergyLabel>(json);
        read.Should().Throw<Exception>().Subject.Single().Should().BeOfType(exceptionType);
    }
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.EnergyLabel);
        xml.Should().Be("A++");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<EnergyLabel>("A++");
        svo.Should().Be(Svo.EnergyLabel);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.EnergyLabel);
        Svo.EnergyLabel.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.EnergyLabel);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.EnergyLabel;
        obj.GetSchema().Should().BeNull();
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(EnergyLabel))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(EnergyLabel),
           description: "EU energy label",
           example: "A++",
           type: "string",
           format: "energy-label",
           pattern: @"[A-H]|A\+{1,4}",
           nullable: true));

    [TestCase("G")]
    [TestCase("B")]
    [TestCase("A")]
    [TestCase("A+")]
    [TestCase("A++++")]
    public void pattern_matches(string input)
        => OpenApiDataType.FromType(typeof(EnergyLabel))!.Matches(input).Should().BeTrue();
}

#if NET8_0_OR_GREATER
#else
public class Supports_binary_serialization
{
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void using_BinaryFormatter()
    {
        var round_tripped = SerializeDeserialize.Binary(Svo.EnergyLabel);
        round_tripped.Should().Be(Svo.EnergyLabel);
    }

    [Test]
    public void storing_int_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.EnergyLabel);
        info.GetInt32("Value").Should().Be(9);
    }
}
#endif

public class Debugger
{
    [TestCase("{empty}", "")]
    [TestCase("{unknown}", "?")]
    [TestCase("A++", "A++")]
    public void has_custom_display(object display, EnergyLabel svo)
        => svo.Should().HaveDebuggerDisplay(display);
}
