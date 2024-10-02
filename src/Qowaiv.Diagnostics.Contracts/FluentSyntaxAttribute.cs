namespace Qowaiv.Diagnostics.Contracts;

/// <summary>To mark a method explicitly as impure. Methods decorated with
/// this attribute return the same instance that was provided as argument.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class FluentSyntaxAttribute(string? justification = null) : ImpureAttribute(justification) { }
