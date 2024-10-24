namespace Qowaiv.Diagnostics.Contracts
{
    /// <summary>Indicates the enum is empty by design.</summary>
    [System.AttributeUsage(System.AttributeTargets.Enum, AllowMultiple = false)]
    [System.Diagnostics.Conditional("CONTRACTS_FULL")]
    internal sealed class EmptyTestEnumAttribute(string? justification) : Qowaiv.Diagnostics.Contracts.EmptyEnumAttribute(justification ?? "For test purposes.")
    {
        public EmptyTestEnumAttribute() : this(null) { }
    }
}
