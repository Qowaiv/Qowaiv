using Specs_Generated;

namespace Specs.Customization.ID_string_specs;

public class Bytes
{
    [Test]
    public void describe_the_id()
        => Svo.Generated.StringId.ToByteArray()
        .Should()
        .BeEquivalentTo<byte>([0x51, 0x6F, 0x77, 0x61, 0x69, 0x76, 0x2D, 0x49, 0x44]);

    [Test]
    public void init_the_id()
        => StringBasedId.FromBytes([0x51, 0x6F, 0x77, 0x61, 0x69, 0x76, 0x2D, 0x49, 0x44])
        .Should().Be(Svo.Generated.StringId);
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Generated.StringId.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_StringId_as_object()
    {
        object obj = Svo.Generated.StringId;
        Svo.Generated.StringId.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_StringId_only()
        => new object().Invoking(Svo.Generated.StringId.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            StringBasedId.Empty,
            StringBasedId.Parse("33ef5805c472"),
            StringBasedId.Parse("58617a652a14"),
            StringBasedId.Parse("853634b4e474"),
            StringBasedId.Parse("93ca7b438fb3"),
            StringBasedId.Parse("f5e6c39aadcf"),
        };

        var list = new List<StringBasedId> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(StringBasedId).Should().BeDecoratedWith<TypeConverterAttribute>();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<StringBasedId>().Should().Be(StringBasedId.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<StringBasedId>().Should().Be(StringBasedId.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("Qowaiv-ID").To<StringBasedId>().Should().Be(Svo.Generated.StringId);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Generated.StringId).Should().Be("Qowaiv-ID");
        }
    }
}
