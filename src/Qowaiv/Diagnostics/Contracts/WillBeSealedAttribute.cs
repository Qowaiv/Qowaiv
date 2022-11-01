namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class will be sealed with the next major change.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class WillBeSealedAttribute : InheritableAttribute { }
