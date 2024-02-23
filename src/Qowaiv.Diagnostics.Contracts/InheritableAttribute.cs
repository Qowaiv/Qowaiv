namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the a class is designed to be inheritable.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public class InheritableAttribute(string? justification = null) : Attribute
{
    /// <summary>The justification of this decoration.</summary>
    public string? Justification { get; init; } = justification;
}
