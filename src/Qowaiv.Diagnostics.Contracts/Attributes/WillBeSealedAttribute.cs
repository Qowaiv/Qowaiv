#pragma warning disable

namespace Qowaiv.Diagnostics.Contracts
{
    /// <summary>Indicates the class will be sealed with the next major change.</summary>
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method | System.AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    [System.Diagnostics.Conditional("CONTRACTS_FULL")]
    internal sealed class WillBeSealedAttribute(string? justification) : Qowaiv.Diagnostics.Contracts.InheritableAttribute(justification)
    {
        public WillBeSealedAttribute() : this(null) { }
    }
}
