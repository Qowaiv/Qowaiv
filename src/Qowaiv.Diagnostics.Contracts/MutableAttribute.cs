namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the class is designed to be mutable.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class MutableAttribute : Attribute
{
    /// <summary>Initializes a new instance of the <see cref="MutableAttribute"/> class.</summary>
    public MutableAttribute(string? message = null) => _ = message;
}
