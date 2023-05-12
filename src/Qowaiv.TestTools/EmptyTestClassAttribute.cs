namespace Qowaiv.TestTools;

/// <summary>
/// Indicates that this class exists for test purposes only, in most cases to
/// create a specific type, without the need of properties of methods.
/// </summary>
/// <remarks>
/// Using this attribute prevents S2094 (Classes should not be empty) from
/// showing up.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class EmptyTestClassAttribute : Attribute
{
    /// <summary>(Optional) justification for having this empty test class.</summary>
    public string? Justification { get; set; }
}
