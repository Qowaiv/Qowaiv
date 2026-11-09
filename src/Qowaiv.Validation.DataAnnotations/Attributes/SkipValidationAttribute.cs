namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Indicates that validation should be skipped.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class SkipValidationAttribute(string? justification) : Attribute
{
    /// <summary>Initializes a new instance of the <see cref="SkipValidationAttribute"/> class.</summary>
    public SkipValidationAttribute() : this(null) { }

    /// <summary>Justification for skipping the validation.</summary>
    public string? Justification { get; } = justification;
}
