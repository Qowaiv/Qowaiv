using Qowaiv.Reflection;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>The exception that is thrown if a type is not supported.</summary>
public class UnsupportedType : NotSupportedException
{
    /// <summary>Initializes a new instance of the <see cref="UnsupportedType"/> class.</summary>
    [ExcludeFromCodeCoverage/* Justification = Required for extensibility. */]
    public UnsupportedType() { }

    /// <summary>Initializes a new instance of the <see cref="UnsupportedType"/> class.</summary>
    public UnsupportedType(string? message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="UnsupportedType"/> class.</summary>
    [ExcludeFromCodeCoverage/* Justification = Required for extensibility. */]
    public UnsupportedType(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <summary>Type is not supported by the <see cref="ValidationAttribute"/>.</summary>
    [Pure]
    public static UnsupportedType ForAttribute<TAttribute>(Type type) where TAttribute : ValidationAttribute
        => new(string.Format(
            QowaivValidationMessages.UnsupportedType_ForAttribute,
            typeof(TAttribute).ToCSharpString(),
            type.ToCSharpString()));
}
