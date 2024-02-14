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

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    
    [TestCase("", "")]
    [TestCase(12345678L, 12345678)]
    [TestCase("12345678", 12345678)]
    public void System_Text_JSON_deserialization(object json, Int32Id svo)
        => JsonTester.Read_System_Text_JSON<Int32Id>(json).Should().Be(svo);

    [TestCase("", null)]
    [TestCase("12345678", 12345678L)]
    public void System_Text_JSON_serialization(Int32Id svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);

    [TestCase(-2)]
    [TestCase(17)]
    [TestCase("17")]
    [TestCase(int.MaxValue + 1L)]
    public void taking_constrains_into_account(object json)
    {
        json.Invoking(JsonTester.Read_System_Text_JSON<Id<ForEven>>)
            .Should().Throw<System.Text.Json.JsonException>()
            .WithMessage("Not a valid identifier.");
    }

    private sealed class ForEven : Int32IdBehavior
    {
        public override bool TryCreate(object? obj, out object? id)
        {
            if(obj is int even && even % 2 == 0)
            {
                id = even;
                return true;
            }
            else
            {
                id = null;
                return false;
            }
        }

        public override bool TryParse(string? str, out object? id)
            => TryCreate(int.TryParse(str, out int even) ? even : null, out id);
    }

#endif
    [TestCase("", "")]
    [TestCase(12345678L, 12345678)]
    [TestCase("12345678", 12345678)]
    public void convention_based_deserialization(object json, Int32Id svo)
        => JsonTester.Read<Int32Id>(json).Should().Be(svo);

    [TestCase("", null)]
    [TestCase("12345678", 12345678L)]
    public void convention_based_serialization(Int32Id svo, object json)
        => JsonTester.Write(svo).Should().Be(json);
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Int32Id).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<Int32Id>().Should().Be(Int32Id.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<Int32Id>().Should().Be(Int32Id.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("PREFIX17").To<Int32Id>().Should().Be(Svo.Int32Id);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
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

#if NET8_0_OR_GREATER
#else
public class Supports_binary_serialization
{
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void using_BinaryFormatter()
    {
        var round_tripped = SerializeDeserialize.Binary(Svo.Int32Id);
        round_tripped.Should().Be(Svo.Int32Id);
    }

    [Test]
    public void storing_value_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.Int32Id);
        info.GetInt32("Value").Should().Be(17);
    }
}
#endif

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
