namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class is designed to be mutable.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class MutableAttribute(string? justification = null) : Attribute
{
    /// <summary>The justification of this decoration.</summary>
    public string? Justification { get; init; } = justification;
}
