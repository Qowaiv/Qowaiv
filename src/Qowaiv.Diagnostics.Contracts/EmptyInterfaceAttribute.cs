namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class is empty by design.</summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public class EmptyInterfaceAttribute(string justification) : EmptyTypeAttribute(justification) { }
