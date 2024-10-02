namespace Qowaiv.Diagnostics.Contracts;

/// <summary>To mark a method explicitly as impure. Methods decorated with
/// this attribute return info about (like, removal or addition was successful).
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class CollectionMutationAttribute(string? justification = null) : ImpureAttribute(justification) { }
