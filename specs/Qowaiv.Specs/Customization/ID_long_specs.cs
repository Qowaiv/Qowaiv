using Specs_Generated;

namespace Specs.Customization.ID_long_specs;

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Generated.Int64Id.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_Int64Id_as_object()
    {
        object obj = Svo.Generated.Int64Id;
        Svo.Generated.Int64Id.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Int64Id_only()
        => new object().Invoking(Svo.Generated.Int64Id.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            Int64BasedId.Empty,
            Int64BasedId.Create(1L),
            Int64BasedId.Create(3L),
            Int64BasedId.Create(7L),
            Int64BasedId.Create(11L),
            Int64BasedId.Create(17L),
        };

        var list = new List<Int64BasedId> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

#if NET6_0_OR_GREATER
public class Supports_JSON_serialization
{
    [TestCase("", null)]
    [TestCase(null, null)]
    [TestCase(123456789L, 123456789L)]
    [TestCase("123456789", 123456789L)]
    public void System_Text_JSON_deserialization(object json, Int64BasedId svo)
        => JsonTester.Read_System_Text_JSON<Int64BasedId>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase(123456789L, 123456789)]
    public void System_Text_JSON_serialization(Int64BasedId svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
}
#endif

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Int64BasedId).Should().BeDecoratedWith<TypeConverterAttribute>();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<Int64BasedId>().Should().Be(Int64BasedId.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<Int64BasedId>().Should().Be(Int64BasedId.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("PREFIX987654321").To<Int64BasedId>().Should().Be(Svo.Generated.Int64Id);
        }
    }

    [Test]
    public void to_string()
        => Converting.ToString().From(Svo.Generated.Int64Id).Should().Be("PREFIX987654321");

    [Test]
    public void from_long()
        => Converting.From(987654321L).To<Int64BasedId>().Should().Be(Svo.Generated.Int64Id);

    [Test]
    public void to_long()
        => Converting.To<long>().From(Svo.Generated.Int64Id).Should().Be(987654321L);

    [TestCase("8a1a8c42-d2ff-e254-e26e-b6abcbf19420")]
    [TestCase("PREF17")]
    public void from_invalid_string(string str)
    {
        Func<Int64BasedId> convert = () => Converting.From(str).To<Int64BasedId>();
        convert.Should().Throw<InvalidCastException>()
            .WithMessage("Cast from string to Specs_Generated.Int64Id is not valid.");
    }

    [Test]
    public void from_invalid_number()
    {
        Func<Int64BasedId> convert = () => Converting.From(-18L).To<Int64BasedId>();
        convert.Should().Throw<InvalidCastException>()
            .WithMessage("Cast from long to Specs_Generated.Int64Id is not valid.");
    }
}
