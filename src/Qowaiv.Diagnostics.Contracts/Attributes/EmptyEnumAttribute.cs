using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the enum is empty by design.</summary>
[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
internal class EmptyEnumAttribute(string justification) : EmptyTypeAttribute(justification) { }
