namespace YearSpan_specs;

public class Has_constant
{
    [Test]
    public void Zero() => YearSpan.Zero.Should().Be(default);

    [Test]
    public void MinValue() => YearSpan.MinValue.Should().Be(YearSpan.Create(-9999));

    [Test]
    public void MaxValue() => YearSpan.MaxValue.Should().Be(YearSpan.Create(+9999));
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.YearSpan.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.YearSpan.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.YearSpan.Equals(YearSpan.Create(42)).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.YearSpan.Equals(YearSpan.Create(17)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.YearSpan == YearSpan.Create(17)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.YearSpan == YearSpan.Create(42)).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.YearSpan != YearSpan.Create(17)).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.YearSpan != YearSpan.Create(42)).Should().BeTrue();

    [TestCase("0", 0)]
    [TestCase("17", 665630146)]
    public void hash_code_is_value_based(YearSpan svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}

public class Can_be_parsed
{
    [TestCase("en", "17")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            YearSpan.Parse(input).Should().Be(Svo.YearSpan);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Func<YearSpan> parse = () => YearSpan.Parse("invalid input");
            parse.Should().Throw<FormatException>()
                .WithMessage("Not a valid year span");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => (YearSpan.TryParse("invalid input", out _)).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => YearSpan.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => YearSpan.TryParse("17").Should().Be(Svo.YearSpan);
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.YearSpan.ToString().Should().Be("17");
        }
    }

    [Test]
    public void with_null_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.YearSpan.ToString().Should().Be(Svo.YearSpan.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.YearSpan.ToString().Should().Be(Svo.YearSpan.ToString(string.Empty));
        }
    }

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.YearSpan.ToString(FormatProvider.Empty).Should().Be("17");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.YearSpan.ToString("#_00_00_0", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '_00_01_7', format: '#_00_00_0'");
    }

    [TestCase("en-GB", null, "17", "17")]
    [TestCase("nl-BE", "0.0", "17", "17,0")]
    public void culture_dependent(CultureInfo culture, string format, YearSpan svo, string formatted)
    {
        using (culture.Scoped())
        {
            svo.ToString(format).Should().Be(formatted);
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.YearSpan.ToString(provider: null).Should().Be("17");
        }
    }

    public class Is_comparable
    {
        [Test]
        public void to_null() => Svo.YearSpan.CompareTo(null).Should().Be(1);

        [Test]
        public void to_YearSpan_as_object()
        {
            object obj = Svo.YearSpan;
            Svo.YearSpan.CompareTo(obj).Should().Be(0);
        }

        [Test]
        public void to_YearSpan_only()
            =>  new object().Invoking(o => Svo.YearSpan.CompareTo(o)).Should().Throw<ArgumentException>();

        [Test]
        public void can_be_sorted_using_compare()
        {
            YearSpan[] sorted =
            [
                default,
                default,
                1.Years(),
                2.Years(),
                3.Years(),
                4.Years(),
            ];

            var list = new List<YearSpan> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
            list.Sort();
            list.Should().BeEquivalentTo(sorted);
        }

        [Test]
        public void by_operators_for_different_values()
        {
            var smaller = YearSpan.Create(17);
            var bigger = YearSpan.Create(18);

            (smaller < bigger).Should().BeTrue();
            (smaller <= bigger).Should().BeTrue();
            (smaller > bigger).Should().BeFalse();
            (smaller >= bigger).Should().BeFalse();
        }

        [Test]
        public void by_operators_for_equal_values()
        {
            var left = YearSpan.Create(17);
            var right = YearSpan.Create(17);

            (left < right).Should().BeFalse();
            (left <= right).Should().BeTrue();
            (left > right).Should().BeFalse();
            (left >= right).Should().BeTrue();
        }
    }

    public class Casts
    {
        [Test]
        public void explicitly_from_int()
        {
            var casted = (YearSpan)17;
            casted.Should().Be(Svo.YearSpan);
        }

        [Test]
        public void explicitly_to_int()
        {
            var casted = (int)Svo.YearSpan;
            casted.Should().Be(17);
        }
    }

    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
            => typeof(YearSpan).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.en_GB.Scoped())
            {
                Converting.From<string>(null).To<YearSpan>().Should().Be(YearSpan.Zero);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.en_GB.Scoped())
            {
                Converting.From("17").To<YearSpan>().Should().Be(Svo.YearSpan);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.en_GB.Scoped())
            {
                Converting.ToString().From(Svo.YearSpan).Should().Be("17");
            }
        }

        [Test]
        public void from_int()
            => Converting.From(17).To<YearSpan>().Should().Be(Svo.YearSpan);

        [Test]
        public void to_int()
            => Converting.To<int>().From(Svo.YearSpan).Should().Be(17);
    }

    public class Supports_JSON_serialization
    {
        [TestCase(17, 17.0)]
        [TestCase(17, 17L)]
        [TestCase(17, "17")]
        public void convention_based_deserialization(YearSpan svo, object json)
            => JsonTester.Read<YearSpan>(json).Should().Be(svo);

        [TestCase(17, 17)]
        public void convention_based_serialization(object json, YearSpan svo)
            => JsonTester.Write(svo).Should().Be(json);

        [TestCase("Invalid input", typeof(FormatException))]
        [TestCase("2017-06-11", typeof(FormatException))]
        public void throws_for_invalid_json(object json, Type exceptionType)
        {
            Func<YearSpan> read = () => JsonTester.Read<YearSpan>(json);
            read.Should().Throw<Exception>().Subject.Single().Should().BeOfType(exceptionType);
        }
    }

    public class Supports_XML_serialization
    {
        [Test]
        public void using_XmlSerializer_to_serialize()
        {
            var xml = Serialize.Xml(Svo.YearSpan);
            xml.Should().Be("17");
        }

        [Test]
        public void using_XmlSerializer_to_deserialize()
        {
            var svo = Deserialize.Xml<YearSpan>("17");
            svo.Should().Be(Svo.YearSpan);
        }

        [Test]
        public void using_DataContractSerializer()
        {
            var round_tripped = SerializeDeserialize.DataContract(Svo.YearSpan);
            Svo.YearSpan.Should().Be(round_tripped);
        }

        [Test]
        public void as_part_of_a_structure()
        {
            var structure = XmlStructure.New(Svo.YearSpan);
            var round_tripped = SerializeDeserialize.Xml(structure);
            structure.Should().Be(round_tripped);
        }

        [Test]
        public void has_no_custom_XML_schema()
        {
            IXmlSerializable obj = Svo.YearSpan;
            obj.GetSchema().Should().BeNull();
        }
    }

    public class Is_Open_API_data_type
    {
        [Test]
        public void with_info()
           => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(YearSpan))
           .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
               dataType: typeof(YearSpan),
               description: "Year span",
               example: 17,
               type: "int",
               format: "year-span",
               pattern: null));
    }

    public class Debugger
    {
        [Test]
        public void has_custom_display()
            => Svo.YearSpan.Should().HaveDebuggerDisplay("17 years");
    }
}
