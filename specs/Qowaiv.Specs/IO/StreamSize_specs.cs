namespace IO.StreamSize_specs;

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.StreamSize.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_StreamSize_as_object()
    {
        object obj = Svo.StreamSize;
        Svo.StreamSize.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_StreamSize_only()
        => new object().Invoking(Svo.StreamSize.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        StreamSize[] sorted =
        [
            StreamSize.Zero,
            StreamSize.Zero,
            13.465.KB(),
            83.465.KB(),
            11.346.MB(),
            77.346.MB(),
        ];

        var list = new List<StreamSize> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();

        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        StreamSize smaller = 17;
        StreamSize bigger = 19;

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        StreamSize left = 17;
        StreamSize right = 17;

        (left < right).Should().BeFalse();
        (left <= right).Should().BeTrue();
        (left > right).Should().BeFalse();
        (left >= right).Should().BeTrue();
    }
}

public class Can_be_parsed
{
    [TestCase("en", "123456789")]
    [TestCase("en", "123456.789 kB")]
    [TestCase("en", "123456.789 kilobyte")]
    [TestCase("en", "123.456789 MB")]
    [TestCase("nl", "123,456789 MB")]
    [TestCase("nl", "0,123456789 GB")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            StreamSize.Parse(input).Should().Be(Svo.StreamSize);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Func<StreamSize> parse = () => StreamSize.Parse("invalid input");
            parse.Should().Throw<FormatException>().WithMessage("Not a valid stream size");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => (StreamSize.TryParse("invalid input", out _)).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => StreamSize.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => StreamSize.TryParse("123456789").Should().Be(Svo.StreamSize);
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.StreamSize.ToString().Should().Be("123456789 byte");
        }
    }

    [Test]
    public void with_null_format()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.StreamSize.ToString(default(string)).Should().Be("123456789");
        }
    }

    [Test]
    public void with_string_empty_pattern()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.StreamSize.ToString(string.Empty).Should().Be("123456789");
        }
    }

    [Test]
    public void default_value_is_represented_as_zero()
    {
        using (TestCultures.en_GB.Scoped())
        {
            default(StreamSize).ToString().Should().Be("0 byte");
        }
    }

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.StreamSize.ToString(FormatProvider.Empty).Should().Be("123456789 byte");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.StreamSize.ToString("0.0 F", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '123.5 Megabyte', format: '0.0 F'");
    }

    [TestCase("nl-NL", "#,##0b", 123456789, "123.456.789b")]
    [TestCase("nl-NL", "#,##0.00 kB", 123456789, "123.456,79 kB")]
    [TestCase("nl-BE", "0.0 MegaByte", 123456789, "123,5 MegaByte")]
    [TestCase("nl-BE", "0.0 F", -123456789, "-123,5 Megabyte")]
    [TestCase("nl-BE", "0.00GB", 123456789, "0,12GB")]
    [TestCase("de-DE", "0.0000 GiB", 123456789, "0,1150 GiB")]
    [TestCase("nl-BE", "tb", 1_000_000_000_000_000L, "1000tb")]
    [TestCase("nl-BE", " petabyte", 1_000_000_000_000L, "0,001 petabyte")]
    [TestCase("nl-BE", "#,##0.## Exabyte", long.MaxValue, "9,22 Exabyte")]
    [TestCase("nl-BE", "#,##0.## F", 123456789, "123,46 Megabyte")]
    [TestCase("nl-BE", "0 f", 123456789, "123 megabyte")]
    [TestCase("nl-BE", "0000 S", 123456789, "0123 MB")]
    [TestCase("nl-BE", "0 s", 123456789, "123 mb")]
    [TestCase("nl-BE", "0s", 123456789, "123mb")]
    [TestCase("nl-BE", "0.0 si", 123456789, "117,7 mib")]
    public void format_dependent(CultureInfo culture, string format, long bytes, string formatted)
    {
        using (culture.Scoped())
        {
            ((StreamSize)bytes).ToString(format).Should().Be(formatted);
        }
    }

    [TestCase("nl-BE", null, "1600,1", "1600 byte")]
    [TestCase("en-GB", null, "1600.1", "1600 byte")]
    [TestCase("nl-BE", "0000 byte", "800", "0800 byte")]
    [TestCase("en-GB", "0000", "800", "0800")]
    [TestCase("es-EC", "00000.0", "1700", "01700,0")]
    public void culture_dependent(CultureInfo culture, string format, string input, string formatted)
    {
        using (culture.Scoped())
        {
            var act = format is null ? StreamSize.Parse(input).ToString() : StreamSize.Parse(input).ToString(format);
            act.Should().Be(formatted);
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.StreamSize.ToString(provider: null).Should().Be("123456789 byte");
        }
    }
}

public class Has_humanizer_creators
{
    [Test]
    public void Bytes_from_int() => 17_000.Bytes().Should().Be(StreamSize.FromKilobytes(17));

    [Test]
    public void Bytes_from_long() => 17_123L.Bytes().Should().Be(StreamSize.FromKilobytes(17.123));

    [Test]
    public void KB_from_int() => 17.KB().Should().Be(StreamSize.FromKilobytes(17));

    [Test]
    public void KB_from_long() => 17L.KB().Should().Be(StreamSize.FromKilobytes(17));

    [Test]
    public void KB_from_double() => 17.4.KB().Should().Be(StreamSize.FromKilobytes(17.4));

    [Test]
    public void MB_from_int() => 17.MB().Should().Be(StreamSize.FromMegabytes(17));

    [Test]
    public void MB_from_long() => 17L.MB().Should().Be(StreamSize.FromMegabytes(17));

    [Test]
    public void MB_from_double() => 17.4.MB().Should().Be(StreamSize.FromMegabytes(17.4));

    [Test]
    public void GB_from_int() => 17.GB().Should().Be(StreamSize.FromGigabytes(17));

    [Test]
    public void GB_from_long() => 17L.GB().Should().Be(StreamSize.FromGigabytes(17));

    [Test]
    public void GB_from_double() => 17.4.GB().Should().Be(StreamSize.FromGigabytes(17.4));

    [Test]
    public void KiB_from_int() => 17.KiB().Should().Be(StreamSize.FromKibibytes(17));

    [Test]
    public void KiB_from_long() => 17L.KiB().Should().Be(StreamSize.FromKibibytes(17));

    [Test]
    public void KiB_from_double() => 17.4.KiB().Should().Be(StreamSize.FromKibibytes(17.4));

    [Test]
    public void MiB_from_int() => 17.MiB().Should().Be(StreamSize.FromMebibytes(17));

    [Test]
    public void MiB_from_long() => 17L.MiB().Should().Be(StreamSize.FromMebibytes(17));

    [Test]
    public void MiB_from_double() => 17.4.MiB().Should().Be(StreamSize.FromMebibytes(17.4));

    [Test]
    public void GiB_from_int() => 17.GiB().Should().Be(StreamSize.FromGibibytes(17));

    [Test]
    public void GiB_from_long() => 17L.GiB().Should().Be(StreamSize.FromGibibytes(17));

    [Test]
    public void GiB_from_double() => 17.4.GiB().Should().Be(StreamSize.FromGibibytes(17.4));
}

public class Created_from
{
    [Test]
    public void byte_array()
        => new byte[42].GetStreamSize().Should().Be(42.Bytes());

    [Test]
    public void IO_Directory_info()
    {
        using var temp = new TemporaryDirectory();
        DirectoryInfo dir = temp;

        for (var i = 0; i < 10; i++)
        {
            var file = temp.CreateFile($"text_{i}.md");
            using var writer = file.AppendText();
            writer.Write("Hello, world!");
        }
        var size = dir.GetStreamSize();
        size.Should().Be(130.Bytes());
    }

    [Test]
    public void IO_File_info()
    {
        using var dir = new TemporaryDirectory();
        var file = dir.CreateFile($"text.md");
        using var writer = file.AppendText();
        writer.Write("Hello, world!");
        writer.Flush();
        file.GetStreamSize().Should().Be(13.Bytes());
    }
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.StreamSize.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.StreamSize.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.StreamSize.Equals(StreamSize.MinValue).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.StreamSize.Equals(StreamSize.Byte * 123456789).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (StreamSize.Byte * 123456789 == Svo.StreamSize).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (StreamSize.Byte * 123456789 == StreamSize.MinValue).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (StreamSize.Byte * 123456789 != Svo.StreamSize).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (StreamSize.Byte * 123456789 != StreamSize.MinValue).Should().BeTrue();

    [TestCase("0 byte", 0)]
    [TestCase("123456789 byte", 553089222)]
    public void hash_code_is_value_based(StreamSize svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(StreamSize).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<StreamSize>().Should().Be(StreamSize.Zero);
        }
    }

    [TestCase("123456789")]
    [TestCase("123456.789 kB")]
    public void from_string(string str)
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(str).To<StreamSize>().Should().Be(Svo.StreamSize);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.StreamSize).Should().Be("123456789 byte");
        }
    }

    [Test]
    public void from_long()
        => Converting.From(123456789L).To<StreamSize>().Should().Be(Svo.StreamSize);

    [Test]
    public void to_long()
        => Converting.To<long>().From(Svo.StreamSize).Should().Be(123456789);
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase("1600", 1_600)]
    [TestCase("17MB", 17_000_000)]
    [TestCase("1.766Kb", 1_766)]
    [TestCase(1234L, 1234)]
    [TestCase(1258.9, 1258)]
    public void System_Text_JSON_deserialization(object json, StreamSize svo)
        => JsonTester.Read_System_Text_JSON<StreamSize>(json).Should().Be(svo);

    [TestCase(17L, 17L)]
    public void System_Text_JSON_serialization(StreamSize svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("1600", 1_600)]
    [TestCase("17MB", 17_000_000)]
    [TestCase("1.766Kb", 1_766)]
    [TestCase(1234L, 1234)]
    [TestCase(1258.9, 1258)]
    public void convention_based_deserialization(object json, StreamSize svo)
       => JsonTester.Read<StreamSize>(json).Should().Be(svo);

    [TestCase(17L, 17L)]
    public void convention_based_serialization(StreamSize svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<StreamSize>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}
public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => OpenApiDataType.FromType(typeof(StreamSize))
        .Should().Be(new OpenApiDataType(
            dataType: typeof(StreamSize),
            description: "Stream size notation (in byte).",
            example: 1024,
            type: "integer",
            format: "stream-size"));
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.StreamSize);
        xml.Should().Be("123456789 byte");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<StreamSize>("123456789 byte");
        svo.Should().Be(Svo.StreamSize);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.StreamSize);
        Svo.StreamSize.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.StreamSize);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.StreamSize;
        obj.GetSchema().Should().BeNull();
    }
}
