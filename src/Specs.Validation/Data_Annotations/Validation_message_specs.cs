using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.Validation_message_specs;

public class None
{
    [Test]
    public void returns_null() => ValidationMessage.None.Should().BeNull();
}
