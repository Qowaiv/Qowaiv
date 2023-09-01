namespace Identifiers.Id_for_Int32_specs;

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Int32Id.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_Int32Id_as_object()
    {
        object obj = Svo.Int32Id;
        Svo.Int32Id.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Int32Id_only()
        => Assert.Throws<ArgumentException>(() => Svo.Int32Id.CompareTo(new object()));

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            default,
            Int32Id.Create(1),
            Int32Id.Create(2),
            Int32Id.Create(3),
            Int32Id.Create(4),
            Int32Id.Create(17),
        };

        var list = new List<Int32Id> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}
public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Int32Id).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.FromNull<string>().To<Int32Id>().Should().Be(Int32Id.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<Int32Id>().Should().Be(Int32Id.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("PREFIX17").To<Int32Id>().Should().Be(Svo.Int32Id);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.Int32Id).Should().Be("17");
        }
    }

    [Test]
    public void from_int()
        => Converting.From(17).To<Int32Id>().Should().Be(Svo.Int32Id);

    [Test]
    public void to_int()
        => Converting.To<int>().From(Svo.Int32Id).Should().Be(17);
}

public class Supports_binary_serialization
{
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void using_BinaryFormatter()
    {
        var round_tripped = SerializeDeserialize.Binary(Svo.Int32Id);
        Svo.Int32Id.Should().Be(round_tripped);
    }

    [Test]
    public void storing_value_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.Int32Id);
        info.GetInt32("Value").Should().Be(17);
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(ForInt32))
        .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
            dataType: typeof(Int32Id),
            description: "Int32 based identifier",
            example: 17,
            type: "integer",
            format: "identifier",
            nullable: true));
}
