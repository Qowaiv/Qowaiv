using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the struct is empty by design.</summary>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
internal sealed class EmptyTestStructAttribute(string? justification = null) : EmptyStructAttribute(justification ?? "For test purposes.") { }
