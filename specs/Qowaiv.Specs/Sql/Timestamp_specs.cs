namespace Sql.Timestamp_specs;

public class Is_invalid
{
    [Test]
    public void for_empty_string() => Timestamp.TryParse(string.Empty).Should().BeNull();

    [Test]
    public void for_null() => Timestamp.TryParse(Nil.String).Should().BeNull();

    [Test]
    public void for_garbage() => Timestamp.TryParse("Not a timestamp").Should().BeNull();
}

public class Can_be_parsed
{
    [TestCase("en-GB", "0x00000000499602D2")]
    [TestCase("en-GB", "1234567890")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            Timestamp.Parse(input).Should().Be(Svo.Timestamp);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            "invalid input".Invoking(Timestamp.Parse)
                .Should().Throw<FormatException>()
                .WithMessage("Not a valid SQL timestamp");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => Timestamp.TryParse("0xInvalidTimeStamp", out _).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Timestamp.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => Timestamp.TryParse("1234567890").Should().Be(Svo.Timestamp);
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Timestamp.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Timestamp.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Timestamp.Equals(Timestamp.MinValue).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Timestamp.Equals(Timestamp.Create(1234567890L)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Timestamp.Create(1234567890L) == Svo.Timestamp).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Timestamp.Create(1234567890L) == Timestamp.MinValue).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Timestamp.Create(1234567890L) != Svo.Timestamp).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Timestamp.Create(1234567890L) != Timestamp.MinValue).Should().BeTrue();

    [Test]
    public void formatted_and_unformatted_are_equal()
    {
        var l = Timestamp.Parse("0x75bcd15", CultureInfo.InvariantCulture);
        var r = Timestamp.Parse("0x00000000075BCD15", CultureInfo.InvariantCulture);
        l.Equals(r).Should().BeTrue();
    }

    [TestCase("0", 0)]
    [TestCase("1234567890", 1849341697)]
    public void hash_code_is_value_based(Timestamp svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
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
            Svo.Timestamp.ToString().Should().Be("0x00000000499602D2");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Timestamp.ToString(default(string)).Should().Be(Svo.Timestamp.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Timestamp.ToString(string.Empty).Should().Be(Svo.Timestamp.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_0x0000000000000000()
        => default(Timestamp).ToString().Should().Be("0x0000000000000000");

    [Test]
    public void max_value_is_represented_as_0xFFFFFFFFFFFFFFFF()
        => Timestamp.MaxValue.ToString().Should().Be("0xFFFFFFFFFFFFFFFF");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.Timestamp.ToString(FormatProvider.Empty).Should().Be("0x00000000499602D2");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = ((Timestamp)123456789L).ToString("#,##0", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '123,456,789', format: '#,##0'");
    }

    [TestCase("nl-BE", null, "1600", "0x0000000000000640")]
    [TestCase("nl-BE", "0000", "800", "0800")]
    [TestCase("en-GB", "0000", "800", "0800")]
    [TestCase("es-EC", "00000.0", "1700", "01700,0")]
    public void culture_dependent(CultureInfo culture, string format, string input, string formatted)
    {
        using (culture.Scoped())
        {
            Timestamp.Parse(input).ToString(format).Should().Be(formatted);
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.Timestamp.ToString(provider: null).Should().Be("0x00000000499602D2");
        }
    }
}

public class Casts
{
    [Test]
    public void explicitly_from_byte_array()
    {
        var casted = (Timestamp)Svo.Timestamp.ToByteArray();
        casted.Should().Be(Svo.Timestamp);
    }

    [Test]
    public void explicitly_to_byte_array()
    {
        var casted = (byte[])Svo.Timestamp;
        casted.Should().BeEquivalentTo(Svo.Timestamp.ToByteArray());
    }

    [Test]
    public void explicitly_from_long()
    {
        var casted = (Timestamp)1234567890L;
        casted.Should().Be(Svo.Timestamp);
    }

    [Test]
    public void explicitly_to_long()
    {
        var casted = (long)Svo.Timestamp;
        casted.Should().Be(1234567890L);
    }

    [Test]
    public void implicitly_from_ulong()
    {
        Timestamp casted = 1234567890UL;
        casted.Should().Be(Svo.Timestamp);
    }

    [Test]
    public void explicitly_to_ulong()
    {
        var casted = (ulong)Svo.Timestamp;
        casted.Should().Be(1234567890UL);
    }
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Timestamp.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_Timestamp_as_object()
    {
        object obj = Svo.Timestamp;
        Svo.Timestamp.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Timestamp_only()
        => new object().Invoking(Svo.Timestamp.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        Timestamp[] sorted =
        [
            Timestamp.MinValue,
            Timestamp.MinValue,
            3245,
            13245,
            132456,
            1324589,
        ];

        var list = new List<Timestamp> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();

        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        Timestamp smaller = 17;
        Timestamp bigger = 19;

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        Timestamp left = 17;
        Timestamp right = 17;

        (left < right).Should().BeFalse();
        (left <= right).Should().BeTrue();
        (left > right).Should().BeFalse();
        (left >= right).Should().BeTrue();
    }
}

public class Can_be_created
{
    [Test]
    public void from_byte_arrays_with_length_8()
        => Timestamp.Create([1, 2, 3, 4, 5, 6, 7, 8]).Should().Be((Timestamp)578437695752307201L);

    [Test]
    public void from_negative_numbers()
        => Timestamp.Create(-23).Should().Be((Timestamp)18446744073709551593L);
}

public class Can_not_be_created
{
    [TestCase(7)]
    [TestCase(9)]
    public void form_byte_arrays_with_a_length_other_than_8(int length)
    {
        Func<Timestamp> create = () => Timestamp.Create(new byte[length]);
        create.Should().Throw<ArgumentException>()
           .WithMessage("The byte array should have size of 8.*");
    }
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase(null, null)]
    [TestCase(1849341697d, "0x000000006E3AB701")]
    [TestCase(1849341697L, "0x000000006E3AB701")]
    [TestCase("1849341697", "0x000000006E3AB701")]
    [TestCase("0x000000006E3AB701", "0x000000006E3AB701")]
    public void System_Text_JSON_deserialization(object json, Timestamp svo)
        => JsonTester.Read_System_Text_JSON<Timestamp>(json).Should().Be(svo);

    [TestCase(null, "0x0000000000000000")]
    [TestCase("0x000000006E3AB701", "0x000000006E3AB701")]
    public void System_Text_JSON_serialization(Timestamp svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase(1849341697d, "0x000000006E3AB701")]
    [TestCase(1849341697L, "0x000000006E3AB701")]
    [TestCase("1849341697", "0x000000006E3AB701")]
    [TestCase("0x000000006E3AB701", "0x000000006E3AB701")]
    public void convention_based_deserialization(object json, Timestamp svo)
        => JsonTester.Read<Timestamp>(json).Should().Be(svo);

    [TestCase(null, "0x0000000000000000")]
    [TestCase("0x000000006E3AB701", "0x000000006E3AB701")]
    public void convention_based_serialization(Timestamp svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<Timestamp>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => OpenApiDataType.FromType(typeof(Timestamp))
        .Should().Be(new OpenApiDataType(
            dataType: typeof(Timestamp),
            description: "SQL Server timestamp notation.",
            example: "0x00000000000007D9",
            type: "string",
            format: "timestamp"));
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.Timestamp);
        xml.Should().Be("0x00000000499602D2");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<Timestamp>("0x00000000499602D2");
        svo.Should().Be(Svo.Timestamp);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.Timestamp);
        Svo.Timestamp.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Timestamp);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Timestamp;
        obj.GetSchema().Should().BeNull();
    }
}
