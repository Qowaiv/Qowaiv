using Qowaiv.Validation.Abstractions;

namespace Abstractions.Validation_message_specs;

public class None
{
    [Test]
    public void Has_an_empty_string_representation() 
        => ValidationMessage.None.Message.Should().BeNull();
}

#if NET8_0_OR_GREATER
#else
public class Serialization
{
    [Test, Obsolete("Binary serialization is considered harmful.")]
    public void On_an_info_message_keeps_all_data()
    {
        var message = ValidationMessage.Info("Can be serialized", "Prop");
        SerializeDeserialize.Binary(message).Should().BeEquivalentTo(message);
    }

    [Test, Obsolete("Binary serialization is considered harmful.")]
    public void On_a_warning_message_keeps_all_data()
    {
        var message = ValidationMessage.Warn("Can be serialized", "Prop");
        SerializeDeserialize.Binary(message).Should().BeEquivalentTo(message);
    }

    [Test, Obsolete("Binary serialization is considered harmful.")]
    public void On_an_error_message_keeps_all_data()
    {
        var message = ValidationMessage.Error("Can be serialized", "Prop");
        SerializeDeserialize.Binary(message).Should().BeEquivalentTo(message);
    }
}
#endif
