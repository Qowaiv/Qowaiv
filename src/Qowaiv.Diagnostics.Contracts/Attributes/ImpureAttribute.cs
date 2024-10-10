using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts;

/// <summary>To mark a method explicitly as impure.</summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
internal class ImpureAttribute(string? justification) : Attribute
{
    public ImpureAttribute() : this(null) { }

    /// <summary>The justification of this decoration.</summary>
    public string? Justification { get; init; } = justification;
}
