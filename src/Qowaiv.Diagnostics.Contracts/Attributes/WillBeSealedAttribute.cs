using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class will be sealed with the next major change.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
internal sealed class WillBeSealedAttribute(string? justification = null) : InheritableAttribute(justification) { }
