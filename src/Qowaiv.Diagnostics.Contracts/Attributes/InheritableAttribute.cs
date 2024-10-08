using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the a class is designed to be inheritable.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
internal class InheritableAttribute(string? justification) : Attribute
{
    public InheritableAttribute() : this(null) { }

    /// <summary>The justification of this decoration.</summary>
    public string? Justification { get; init; } = justification;
}
