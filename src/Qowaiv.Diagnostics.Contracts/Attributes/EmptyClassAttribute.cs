#pragma warning disable

namespace Qowaiv.Diagnostics.Contracts
{
    /// <summary>Indicates the class is empty by design.</summary>
    /// <remarks>
    /// Using this attribute prevents S2094 (Classes should not be empty) from
    /// showing up.
    /// </remarks>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    [System.Diagnostics.Conditional("CONTRACTS_FULL")]
    internal class EmptyClassAttribute(string justification) : Qowaiv.Diagnostics.Contracts.EmptyTypeAttribute(justification) { }
}
