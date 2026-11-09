using Qowaiv.Validation.DataAnnotations;

namespace Data_Annotations.Attributes.Skip_validation_specs;

public class Skips
{
    [Test]
    public void Decorated_property()
        => new DeocratedProperty { Property = null! }
        .ValidateAnnotations()
        .Should().BeValid()
        .WithoutMessages();

    [Test]
    public void Decorated_type()
        => new DecoratedType { Property = null! }
        .ValidateAnnotations()
        .Should().BeValid()
        .WithoutMessages();
}

file class DeocratedProperty
{
    [SkipValidation("Should ingore the required modifier")]
    public required string Property { get; init; }
}

[SkipValidation("Should ingore the required modifier")]
file class DecoratedType
{
    public required string Property { get; init; }
}
