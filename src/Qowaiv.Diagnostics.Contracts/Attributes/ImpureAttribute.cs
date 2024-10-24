namespace Qowaiv.Diagnostics.Contracts;

/// <summary>To mark a method explicitly as impure.</summary>
[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("CONTRACTS_FULL")]
internal class ImpureAttribute(string? justification) : System.Attribute
{
    public ImpureAttribute() : this(null) { }

    /// <summary>The justification of this decoration.</summary>
    public string? Justification { get; init; } = justification;
}
