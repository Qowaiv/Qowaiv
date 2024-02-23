namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the enum is empty by design.</summary>
[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class EmptyTestEnumAttribute(string? justification = null) : EmptyEnumAttribute(justification ?? "For test purposes.") { }
