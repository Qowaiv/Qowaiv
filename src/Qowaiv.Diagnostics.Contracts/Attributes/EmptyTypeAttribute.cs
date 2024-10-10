using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the type is empty by design.</summary>
[Conditional("CONTRACTS_FULL")]
internal abstract class EmptyTypeAttribute(string justification) : Attribute
{
    /// <summary>The justification of this decoration.</summary>
    public string Justification { get; } = justification;
}
