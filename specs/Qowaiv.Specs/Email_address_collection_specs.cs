#if NET8_0_OR_GREATER
#else
namespace Email_address_collection_specs;

public class Supports_binary_serialization
{
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void using_BinaryFormatter()
    {
        var round_tripped = SerializeDeserialize.Binary(Svo.EmailAddressCollection);
        round_tripped.Should().BeEquivalentTo(Svo.EmailAddressCollection);
    }
    [Test]
    public void storing_string_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.EmailAddressCollection);
        info.GetString("Value").Should().Be("info@qowaiv.org,test@qowaiv.org");
    }
}
#endif
