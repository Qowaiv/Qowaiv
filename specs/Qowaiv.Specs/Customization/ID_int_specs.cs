using Specs_Generated;

namespace Specs.Customization.ID_int_specs;

public class With_domain_logic
{
    [Test]
    public void Next_is_not_supported()
    {
        Func<Int32BasedId> next = () => Int32BasedId.Next();
        next.Should().Throw<NotSupportedException>();
    }
}

public class Has_constant
{
    [Test]
    public void Empty_represent_default_value()
        => Int32BasedId.Empty.Should().Be(default);

    [TestCase(true, 17)]
    [TestCase(false, "")]
    public void HasValue_is(bool result, Int32BasedId svo) => svo.HasValue.Should().Be(result);
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Generated.Int32Id.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Generated.Int32Id.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Generated.Int32Id.Equals(Int32BasedId.Create(42)).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Generated.Int32Id.Equals(Int32BasedId.Create(17)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.Generated.Int32Id == Int32BasedId.Create(17)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.Generated.Int32Id == Int32BasedId.Create(42)).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.Generated.Int32Id != Int32BasedId.Create(17)).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.Generated.Int32Id != Int32BasedId.Create(42)).Should().BeTrue();

    [TestCase("", 0)]
    [TestCase(17, 665630146)]
    public void hash_code_is_value_based(Int32BasedId svo, int hashCode)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hashCode);
        }
    }
}


public class Bytes
{
    [Test]
    public void describe_the_id()
        => Svo.Generated.Int32Id.ToByteArray()
        .Should()
        .BeEquivalentTo<byte>([0x11, 0x00, 0x00, 0x00]);

    [Test]
    public void init_the_id()
        => Int32BasedId.FromBytes([0x11, 0x00, 0x00, 0x00])
        .Should().Be(Svo.Generated.Int32Id);
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Generated.Int32Id.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_Int32Id_as_object()
    {
        object obj = Svo.Generated.Int32Id;
        Svo.Generated.Int32Id.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Int32Id_only()
        => new object().Invoking(Svo.Generated.Int32Id.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            default,
            Int32BasedId.Create(1),
            Int32BasedId.Create(2),
            Int32BasedId.Create(3),
            Int32BasedId.Create(4),
            Int32BasedId.Create(17),
        };

        var list = new List<Int32BasedId> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

#if NET6_0_OR_GREATER
public class Supports_JSON_serialization
{
    [TestCase("", "")]
    [TestCase(12345678L, 12345678)]
    [TestCase("PREFIX12345678", 12345678)]
    [TestCase("12345678", 12345678)]
    public void System_Text_JSON_deserialization(object json, Int32BasedId svo)
        => JsonTester.Read_System_Text_JSON<Int32BasedId>(json).Should().Be(svo);

    [TestCase("", null)]
    [TestCase("12345678", 12345678L)]
    public void System_Text_JSON_serialization(Int32BasedId svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);

    [TestCase(-2)]
    [TestCase(17)]
    [TestCase("17")]
    [TestCase(int.MaxValue + 1L)]
    public void taking_constrains_into_account(object json)
    {
        json.Invoking(JsonTester.Read_System_Text_JSON<Specs_Generated.EvenOnlyId>)
            .Should().Throw<System.Text.Json.JsonException>()
            .WithMessage("Not a valid EvenOnlyId");
    }
}
#endif

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Int32BasedId).Should().BeDecoratedWith<TypeConverterAttribute>();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<Int32BasedId>().Should().Be(Int32BasedId.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<Int32BasedId>().Should().Be(Int32BasedId.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("PREFIX17").To<Int32BasedId>().Should().Be(Svo.Generated.Int32Id);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Generated.Int32Id).Should().Be("PREFIX17");
        }
    }

    [Test]
    public void from_int()
        => Converting.From(17).To<Int32BasedId>().Should().Be(Svo.Generated.Int32Id);

    [Test]
    public void to_int()
        => Converting.To<int>().From(Svo.Generated.Int32Id).Should().Be(17);
}
