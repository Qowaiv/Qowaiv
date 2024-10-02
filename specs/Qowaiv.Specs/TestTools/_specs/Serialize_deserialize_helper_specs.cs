namespace TestTools.Serialize_deserialize_helper_specs;

public class Fails_on
{
#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void binary_round_trip_null()
    {
        Func<object> roundtrip = () => SerializeDeserialize.Binary<object>(null!);
        roundtrip.Should().Throw<ArgumentNullException>();
    }
#endif
}
