namespace Qowaiv.Diagnostics.Contracts;

/// <summary>To mark a method explicitly as impure.</summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public class ImpureAttribute(string? justification = null) : Attribute
{
    /// <summary>The justification of this decoration.</summary>
    public string? Justification { get; init; } = justification;
}
