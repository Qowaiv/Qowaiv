namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class will be sealed with the next major change.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class WillBeSealedAttribute(string? message = null) : InheritableAttribute(message) { }
