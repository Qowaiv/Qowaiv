namespace Qowaiv.Diagnostics.Contracts
{
    /// <summary>Indicates the class is designed to be mutable.</summary>
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct | System.AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    [System.Diagnostics.Conditional("CONTRACTS_FULL")]
    internal sealed class MutableAttribute(string? justification) : System.Attribute
    {
        public MutableAttribute() : this(null) { }

        /// <summary>The justification of this decoration.</summary>
        public string? Justification { get; init; } = justification;
    }
}
