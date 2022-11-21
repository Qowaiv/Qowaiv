namespace Identifiers.Id_for_Guid_specs;

public class Is_comparable
{
    [Test]
    public void to_null() => Svo.CustomGuid.CompareTo(null).Should().Be(1);

    [Test]
    public void to_CustomGuid_as_object()
    {
        object obj = Svo.CustomGuid;
        Svo.CustomGuid.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_CustomGuid_only()
        => Assert.Throws<ArgumentException>(() => Svo.CustomGuid.CompareTo(new object()));

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            CustomGuid.Empty,
            CustomGuid.Parse("33ef5805-c472-4b1f-88bb-2f0723c43889"),
            CustomGuid.Parse("58617a65-2a14-4a9a-82a8-c1a82c956c25"),
            CustomGuid.Parse("853634b4-e474-4b0f-b9ba-01fc732b56d8"),
            CustomGuid.Parse("93ca7b43-8fb3-44e5-a21f-feeebb8e0f6f"),
            CustomGuid.Parse("f5e6c39a-adcf-4eca-bcf2-6b8317ac502c"),
        };

        var list = new List<CustomGuid> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

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
            format: "guid",
            nullable: true));
}
