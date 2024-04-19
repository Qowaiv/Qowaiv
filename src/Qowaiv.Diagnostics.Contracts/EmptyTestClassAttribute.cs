﻿namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class is empty by design.</summary>
/// <remarks>
/// Using this attribute prevents S2094 (Classes should not be empty) from
/// showing up.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class EmptyTestClassAttribute(string? justification = null) : EmptyClassAttribute(justification ?? "For test purposes.") { }
