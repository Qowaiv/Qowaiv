namespace Qowaiv.Diagnostics.Contracts;

/// <summary>To mark a method explicitly as impure. Methods decorated with
/// this attribute return the same instance that was provided as argument.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("CONTRACTS_FULL")]
internal sealed class FluentSyntaxAttribute(string? justification) : Qowaiv.Diagnostics.Contracts.ImpureAttribute(justification)
{
    public FluentSyntaxAttribute() : this(null) { }
}
