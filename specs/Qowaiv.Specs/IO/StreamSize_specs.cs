namespace IO.StreamSize_specs;

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
        using (TestCultures.En_GB.Scoped())
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

public class Created_from_IO
{
    [Test]
    public void Directory_info()
    {
        using var dir = new TemporaryDirectory();

        for (var i = 0; i < 10; i++)
        {
            var file = dir.CreateFile($"text_{i}.md");
            using var writer = file.AppendText();
            writer.Write("Hello, world!");
        }
        var size = ((DirectoryInfo)dir).GetStreamSize();
        size.Should().Be(130.Bytes());
    }

    [Test]
    public void File_info()
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
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From<string>(null).To<StreamSize>().Should().Be(StreamSize.Zero);
        }
    }

    [TestCase("123456789")]
    [TestCase("123456.789 kB")]
    public void from_string(string str)
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(str).To<StreamSize>().Should().Be(Svo.StreamSize);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
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
#if NET6_0_OR_GREATER
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
    {
        var exception = Assert.Catch(() => JsonTester.Read<StreamSize>(json));
        Assert.IsInstanceOf(exceptionType, exception);
    }
}
public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(StreamSize))
        .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
            dataType: typeof(StreamSize),
            description: "Stream size notation (in byte).",
            example: 1024,
            type: "integer",
            format: "stream-size"));
}
