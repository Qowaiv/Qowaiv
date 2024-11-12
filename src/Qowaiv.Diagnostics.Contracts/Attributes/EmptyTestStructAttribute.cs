#pragma warning disable

namespace Qowaiv.Diagnostics.Contracts
{
    /// <summary>Indicates the struct is empty by design.</summary>
    [System.AttributeUsage(System.AttributeTargets.Struct, AllowMultiple = false)]
    [System.Diagnostics.Conditional("CONTRACTS_FULL")]
    internal sealed class EmptyTestStructAttribute(string? justification) : Qowaiv.Diagnostics.Contracts.EmptyStructAttribute(justification ?? "For test purposes.")
    {
        public EmptyTestStructAttribute() : this(null) { }
    }
}
