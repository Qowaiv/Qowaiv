namespace Extensions.Guid_specs;

public class Has_UuidVersion
{
    [Test]
    public void of_random_guid() => Guid.NewGuid().UuidVersion.Should().Be(UuidVersion.Random);

    [Test]
    public void empty_for_empty_guid() => Guid.Empty.UuidVersion.Should().Be(UuidVersion.Empty);
}
