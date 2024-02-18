namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the type is empty by design.</summary>
[Conditional("CONTRACTS_FULL")]
public abstract class EmptyTypeAttribute(string justification) : Attribute
{
    /// <summary>THe justification of this decoration.</summary>
    public string Justification { get; } = justification;
}

/// <summary>Indicates the class is empty by design.</summary>
/// <remarks>
/// Using this attribute prevents S2094 (Classes should not be empty) from
/// showing up.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public class EmptyClassAttribute(string justification) : EmptyTypeAttribute(justification) { }

/// <summary>Indicates the enum is empty by design.</summary>
[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public class EmptyEnumAttribute(string justification) : EmptyTypeAttribute(justification) { }

/// <summary>Indicates the class is empty by design.</summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public class EmptyInterfaceAttribute(string justification) : EmptyTypeAttribute(justification) { }

/// <summary>Indicates the struct is empty by design.</summary>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public class EmptyStructAttribute(string justification) : EmptyTypeAttribute(justification) { }

/// <summary>Indicates the class is empty by design.</summary>
/// <remarks>
/// Using this attribute prevents S2094 (Classes should not be empty) from
/// showing up.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class EmptyTestClassAttribute(string? justification = null) : EmptyClassAttribute(justification ?? "For test purposes.") { }

/// <summary>Indicates the enum is empty by design.</summary>
[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class EmptyTestEnumAttribute(string? justification = null) : EmptyEnumAttribute(justification ?? "For test purposes.") { }

/// <summary>Indicates the class is empty by design.</summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class EmptyTestInterfaceAttribute(string? justification = null) : EmptyInterfaceAttribute(justification ?? "For test purposes.") { }

/// <summary>Indicates the struct is empty by design.</summary>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class EmptyTestStructAttribute(string? justification = null) : EmptyStructAttribute(justification ?? "For test purposes.") { }
