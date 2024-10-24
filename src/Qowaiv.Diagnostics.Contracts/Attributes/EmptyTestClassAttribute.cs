namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class is empty by design.</summary>
/// <remarks>
/// Using this attribute prevents S2094 (Classes should not be empty) from
/// showing up.
/// </remarks>
[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
[System.Diagnostics.Conditional("CONTRACTS_FULL")]
internal sealed class EmptyTestClassAttribute(string? justification) : Qowaiv.Diagnostics.Contracts.EmptyClassAttribute(justification ?? "For test purposes.")
{
    public EmptyTestClassAttribute() : this(null) { }
}
