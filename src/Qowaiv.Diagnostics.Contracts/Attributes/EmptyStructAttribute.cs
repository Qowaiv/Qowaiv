using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the struct is empty by design.</summary>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
internal class EmptyStructAttribute(string justification) : EmptyTypeAttribute(justification) { }
