namespace TestTools.Deserialize_helper_specs;

public class Deserializes
{
    [Test]
    public void XML()
        => Deserialize.Xml<int>("17").Should().Be(17);
}

public class Fails_on
{
    [Test]
    public void invalid_XML()
        => "<invalid XML".Invoking(Deserialize.Xml<int>)
        .Should().Throw<SerializationException>();
}
