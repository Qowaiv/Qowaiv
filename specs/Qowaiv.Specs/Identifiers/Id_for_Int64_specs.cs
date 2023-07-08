namespace Identifiers.Id_for_Int64_specs;

public class Is_comparable
{
    [Test]
    public void to_null() => Svo.Int64Id.CompareTo(null).Should().Be(1);

    [Test]
    public void to_Int64Id_as_object()
    {
        object obj = Svo.Int64Id;
        Svo.Int64Id.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Int64Id_only()
        => Assert.Throws<ArgumentException>(() => Svo.Int64Id.CompareTo(new object()));

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            Int64Id.Empty,
            Int64Id.Create(1L),
            Int64Id.Create(3L),
            Int64Id.Create(7L),
            Int64Id.Create(11L),
            Int64Id.Create(17L),
        };

        var list = new List<Int64Id> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}
public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase("", null)]
    [TestCase(null, null)]
    [TestCase(17.0, 017L)]
    [TestCase(123456789L, 123456789L)]
    [TestCase("123456789", 123456789L)]
    public void System_Text_JSON_deserialization(object json, Int64Id svo)
        => JsonTester.Read_System_Text_JSON<Int64Id>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase(123456789L, "123456789")]
    public void System_Text_JSON_serialization(Int64Id svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("", null)]
    [TestCase(123456789L, 123456789L)]
    [TestCase("123456789", 123456789L)]
    public void convention_based_deserialization(object json, Int64Id svo)
        => JsonTester.Read<Int64Id>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase(123456789L, "123456789")]
    public void convention_based_serialization(Int64Id svo, object json)
        => JsonTester.Write(svo).Should().Be(json);
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Int64Id).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.FromNull<string>().To<Int64Id>().Should().Be(Int64Id.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<Int64Id>().Should().Be(Int64Id.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("PREFIX987654321").To<Int64Id>().Should().Be(Svo.Int64Id);
        }
    }

    [Test]
    public void to_string()
        => Converting.ToString().From(Svo.Int64Id).Should().Be("987654321");

    [Test]
    public void from_long()
        => Converting.From(987654321L).To<Int64Id>().Should().Be(Svo.Int64Id);

    [Test]
    public void to_long()
        => Converting.To<long>().From(Svo.Int64Id).Should().Be(987654321L);

    [TestCase("666")]
    [TestCase("PREF17")]
    public void from_invalid_string(string str)
    {
        Func<Int64Id> convert = () => Converting.From(str).To<Int64Id>();
        convert.Should().Throw<InvalidCastException>()
            .WithMessage("Cast from string to Qowaiv.Identifiers.Id<Qowaiv.TestTools.ForInt64> is not valid.");
    }

    [TestCase(-18)]
    [TestCase(666)]
    public void from_invalid_number(long number)
    {
        Func<Int64Id> convert = () => Converting.From(number).To<Int64Id>();
        convert.Should().Throw<InvalidCastException>()
            .WithMessage("Cast from long to Qowaiv.Identifiers.Id<Qowaiv.TestTools.ForInt64> is not valid.");
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(ForInt64))
        .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
            dataType: typeof(Int64Id),
            description: "Int64 based identifier",
            example: 17,
            type: "integer",
            format: "identifier",
            nullable: true));
}
