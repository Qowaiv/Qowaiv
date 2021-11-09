namespace Qowaiv.Identifiers;

/// <summary>Implements <see cref="IIdentifierBehavior"/> for an identifier based on <see cref="Uuid"/>.</summary>
public abstract class UuidBehavior : GuidBehavior
{
    internal new static readonly UuidBehavior Instance = new Default();

    /// <summary>Gets the default format used to represent the <see cref="System.Guid"/> as <see cref="string"/>.</summary>
    protected override string DefaultFormat => "S";

    private sealed class Default : UuidBehavior { }
}
