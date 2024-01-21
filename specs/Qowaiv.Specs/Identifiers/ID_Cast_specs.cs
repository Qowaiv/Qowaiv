namespace Identifiers.Id_specs;

public class Create
{
    [Test]
    public void string_from_null_is_empty()
        => StringId.Create(null).Should().Be(StringId.Empty);

    [Test]
    public void Guid_from_null_is_empty()
        => CustomGuid.Create(null).Should().Be(CustomGuid.Empty);
    [Test]
    public void long_from_null_is_empty()
        => Int64Id.Create(null).Should().Be(Int64Id.Empty);
}

public class Casts_from
{
    [Test]
    public void GUID_to_GUID_based_id()
    {
        var guid = Guid.Parse("AD38ECD4-020F-475C-9318-DFF2067DA1D4");
        var casted = (CustomGuid)guid;
        casted.Should().Be(CustomGuid.Parse("AD38ECD4-020F-475C-9318-DFF2067DA1D4"));
    }

    [Test]
    public void GUID_to_string_based_id()
    {
        var guid = Guid.Parse("ad38ecd4-020f-475c-9318-dff2067da1d4");
        var casted = (StringId)guid;
        casted.Should().Be(StringId.Parse("ad38ecd4-020f-475c-9318-dff2067da1d4"));
    }

    [Test]
    public void long_to_long_based_id()
    {
        var id = 12345L;
        var casted = (Int64Id)id;
        casted.Should().Be(Int64Id.Create(12345L));
    }

    [Test]
    public void long_to_string_based_id()
    {
        var id = 12345L;
        var casted = (StringId)id;
        casted.Should().Be(StringId.Parse("12345"));
    }

    [Test]
    public void string_to_long_based_id()
    {
        var id = "12345";
        var casted = (Int64Id)id;
        casted.Should().Be(Int64Id.Create(12345L));
    }
}

public class Can_not_cast_from
{
    [Test]
    public void invalid_string_for_long()
        => "NaN".Invoking(Int64Id.Create)
            .Should().Throw<InvalidCastException>()
            .WithMessage("Cast from string to Qowaiv.Identifiers.Id<Qowaiv.TestTools.ForInt64> is not valid.");

    [Test]
    public void non_numeric_string_to_long()
        => "ABC".Invoking(id => (Int64Id)id)
            .Should().Throw<InvalidCastException>();

    [Test]
    public void GUID_to_long()
       => Guid.NewGuid().Invoking(id => (Int64Id)id)
           .Should().Throw<InvalidCastException>();

    [Test]
    public void non_GUID_string_to_GUID()
       => "ABC".Invoking(id => (CustomGuid)id)
           .Should().Throw<InvalidCastException>();

    [Test]
    public void long_to_GUID()
       => ((object)123546L).Invoking(id => (CustomGuid)id)
           .Should().Throw<InvalidCastException>();

    [Test]
    public void invalid_JSON_input()
        => (-1L).Invoking(Int64Id.FromJson)
        .Should().Throw<InvalidCastException>();
}

