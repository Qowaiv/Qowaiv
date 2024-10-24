namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class is empty by design.</summary>
[System.AttributeUsage(System.AttributeTargets.Interface, AllowMultiple = false)]
[System.Diagnostics.Conditional("CONTRACTS_FULL")]
internal class EmptyInterfaceAttribute(string justification) : Qowaiv.Diagnostics.Contracts.EmptyTypeAttribute(justification) { }
