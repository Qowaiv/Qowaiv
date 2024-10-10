using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class is designed to be mutable.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
internal sealed class MutableAttribute(string? justification) : Attribute
{
    public MutableAttribute() : this(null) { }

    /// <summary>The justification of this decoration.</summary>
    public string? Justification { get; init; } = justification;
}
