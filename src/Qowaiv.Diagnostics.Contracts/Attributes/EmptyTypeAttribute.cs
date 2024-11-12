#pragma warning disable

namespace Qowaiv.Diagnostics.Contracts
{
    /// <summary>Indicates the type is empty by design.</summary>
    [System.Diagnostics.Conditional("CONTRACTS_FULL")]
    internal abstract class EmptyTypeAttribute(string justification) : System.Attribute
    {
        /// <summary>The justification of this decoration.</summary>
        public string Justification { get; } = justification;
    }
}
