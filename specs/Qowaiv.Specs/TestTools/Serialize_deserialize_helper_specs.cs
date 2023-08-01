namespace TestTools.Serialize_deserialize_helper_specs;

public class Fails_on
{
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void binary_roundtrip_null()
    {
        Func<object> roundtrip = () => SerializeDeserialize.Binary<object>(null!);
        roundtrip.Should().Throw<ArgumentNullException>();
    }
}
