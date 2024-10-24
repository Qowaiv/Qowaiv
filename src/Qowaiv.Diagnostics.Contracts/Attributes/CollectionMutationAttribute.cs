namespace Qowaiv.Diagnostics.Contracts;

/// <summary>To mark a method explicitly as impure. Methods decorated with
/// this attribute return info about (like, removal or addition was successful).
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("CONTRACTS_FULL")]
internal sealed class CollectionMutationAttribute(string? justification = null) : Qowaiv.Diagnostics.Contracts.ImpureAttribute(justification) { }
