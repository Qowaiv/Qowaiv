namespace Qowaiv.Diagnostics.Contracts
{
    /// <summary>Indicates the struct is empty by design.</summary>
    [System.AttributeUsage(System.AttributeTargets.Struct, AllowMultiple = false)]
    [System.Diagnostics.Conditional("CONTRACTS_FULL")]
    internal class EmptyStructAttribute(string justification) : Qowaiv.Diagnostics.Contracts.EmptyTypeAttribute(justification) { }
}
