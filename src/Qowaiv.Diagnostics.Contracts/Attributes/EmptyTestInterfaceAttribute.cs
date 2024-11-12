#pragma warning disable

namespace Qowaiv.Diagnostics.Contracts
{
    /// <summary>Indicates the class is empty by design.</summary>
    [System.AttributeUsage(System.AttributeTargets.Interface, AllowMultiple = false)]
    [System.Diagnostics.Conditional("CONTRACTS_FULL")]
    internal sealed class EmptyTestInterfaceAttribute(string? justification) : Qowaiv.Diagnostics.Contracts.EmptyInterfaceAttribute(justification ?? "For test purposes.")
    {
        public EmptyTestInterfaceAttribute() : this(null) { }
    }
}
