using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class is empty by design.</summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
internal class EmptyInterfaceAttribute(string justification) : EmptyTypeAttribute(justification) { }
