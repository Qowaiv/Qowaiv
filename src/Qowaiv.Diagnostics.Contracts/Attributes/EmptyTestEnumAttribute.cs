using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the enum is empty by design.</summary>
[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
internal sealed class EmptyTestEnumAttribute(string? justification) : EmptyEnumAttribute(justification ?? "For test purposes.")
{
    public EmptyTestEnumAttribute() : this(null) { }
}
