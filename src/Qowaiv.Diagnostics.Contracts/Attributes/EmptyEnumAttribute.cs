namespace Qowaiv.Diagnostics.Contracts
{
    /// <summary>Indicates the enum is empty by design.</summary>
    [System.AttributeUsage(System.AttributeTargets.Enum, AllowMultiple = false)]
    [System.Diagnostics.Conditional("CONTRACTS_FULL")]
    internal class EmptyEnumAttribute(string justification) : Qowaiv.Diagnostics.Contracts.EmptyTypeAttribute(justification) { }
}
