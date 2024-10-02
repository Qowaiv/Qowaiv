namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the type is empty by design.</summary>
[Conditional("CONTRACTS_FULL")]
public abstract class EmptyTypeAttribute(string justification) : Attribute
{
    /// <summary>The justification of this decoration.</summary>
    public string Justification { get; } = justification;
}
