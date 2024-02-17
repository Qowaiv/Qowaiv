namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class is designed to be mutable.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class MutableAttribute : Attribute { }
