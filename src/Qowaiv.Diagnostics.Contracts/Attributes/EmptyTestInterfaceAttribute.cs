using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class is empty by design.</summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
internal sealed class EmptyTestInterfaceAttribute(string? justification = null) : EmptyInterfaceAttribute(justification ?? "For test purposes.") { }
