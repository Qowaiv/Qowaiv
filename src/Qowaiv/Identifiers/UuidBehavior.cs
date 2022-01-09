namespace Qowaiv.Identifiers;

/// <summary>Implements <see cref="IIdentifierBehavior"/> for an identifier based on <see cref="Uuid"/>.</summary>
public class UuidBehavior : GuidBehavior
{
    internal new static readonly UuidBehavior Instance = new();

    /// <summary>Creates a new instance of the <see cref="UuidBehavior"/> class.</summary>
    protected UuidBehavior() { }

    /// <summary>Gets the default format used to represent the <see cref="System.Guid"/> as <see cref="string"/>.</summary>
    protected override string DefaultFormat => "S";
}
