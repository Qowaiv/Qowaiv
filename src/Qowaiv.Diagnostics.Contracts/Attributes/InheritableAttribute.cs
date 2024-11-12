#pragma warning disable

namespace Qowaiv.Diagnostics.Contracts
{
    /// <summary>Indicates the a class is designed to be inheritable.</summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [System.Diagnostics.Conditional("CONTRACTS_FULL")]
    internal class InheritableAttribute(string? justification) : System.Attribute
    {
        public InheritableAttribute() : this(null) { }

        /// <summary>The justification of this decoration.</summary>
        public string? Justification { get; init; } = justification;
    }
}
