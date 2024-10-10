using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the struct is empty by design.</summary>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
internal sealed class EmptyTestStructAttribute(string? justification) : EmptyStructAttribute(justification ?? "For test purposes.")
{
    public EmptyTestStructAttribute() : this(null) { }
}
