namespace Identifiers.Id_for_Guid_specs;

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(CustomGuid).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From<string>(null).To<CustomGuid>().Should().Be(CustomGuid.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<CustomGuid>().Should().Be(CustomGuid.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("8A1A8C42-D2FF-E254-E26E-B6ABCBF19420").To<CustomGuid>().Should().Be(Svo.CustomGuid);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.CustomGuid).Should().Be("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");
        }
    }

    [Test]
    public void from_Guid()
        => Converting.From(Svo.Guid).To<CustomGuid>().Should().Be(Svo.CustomGuid);


    [Test]
    public void from_Uuid()
        => Converting.From(Svo.Uuid).To<CustomGuid>().Should().Be(Svo.CustomGuid);

    [Test]
    public void to_Guid()
        => Converting.To<Guid>().From(Svo.CustomGuid).Should().Be(Svo.Guid);

    [Test]
    public void to_Uuid()
        => Converting.To<Uuid>().From(Svo.CustomGuid).Should().Be(Svo.Uuid);
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(ForGuid))
        .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
            dataType: typeof(CustomGuid),
            description: "GUID based identifier",
            example: "8a1a8c42-d2ff-e254-e26e-b6abcbf19420",
            type: "string",
            format: "guid"));
}
