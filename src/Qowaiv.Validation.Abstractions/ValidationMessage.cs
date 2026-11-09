namespace Qowaiv.Validation.Abstractions;

/// <summary>Implementation of an <see cref="IValidationMessage"/>.</summary>
[Serializable]
public sealed class ValidationMessage : IValidationMessage, IEquatable<ValidationMessage>
{
    /// <summary>Initializes a new instance of the <see cref="ValidationMessage"/> class.</summary>
    public ValidationMessage() : this(ValidationSeverity.None, null, null) { }

    /// <summary>Initializes a new instance of the <see cref="ValidationMessage"/> class.</summary>
    internal ValidationMessage(ValidationSeverity severity, string? message, string? propertyName)
    {
        Severity = severity;
        Message = message;
        PropertyName = propertyName;
    }

    /// <inheritdoc />
    public ValidationSeverity Severity { get; }

    /// <inheritdoc />
    public string? PropertyName { get; }

    /// <inheritdoc />
    public string? Message { get; }

    /// <inheritdoc />
    [Pure]
    public override string ToString()
    {
        if (Equals(None))
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        switch (Severity)
        {
            case ValidationSeverity.Info:
                sb.Append("INF: ");
                break;
            case ValidationSeverity.Warning:
                sb.Append("WRN: ");
                break;
            case ValidationSeverity.Error:
                sb.Append("ERR: ");
                break;
            default:
                sb.Append($"{Severity}: ");
                break;
        }
        if (!string.IsNullOrEmpty(PropertyName))
        {
            sb.Append($"Property: {PropertyName}, ");
        }
        sb.Append(Message);

        return sb.ToString();
    }

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is ValidationMessage other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(ValidationMessage? other)
        => other is not null
        && Severity == other.Severity
        && PropertyName == other.PropertyName
        && Message == other.Message;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode()
        => Severity.GetHashCode()
        ^ (PropertyName ?? string.Empty).GetHashCode()
        ^ (Message ?? string.Empty).GetHashCode();

    /// <summary>Creates a None message.</summary>
    [Pure]
    public static ValidationMessage None => new();

    /// <summary>Creates an error message.</summary>
    [Pure]
    public static ValidationMessage Error(string message, string? propertyName = null) => new(ValidationSeverity.Error, message, propertyName);

    /// <summary>Creates a warning message.</summary>
    [Pure]
    public static ValidationMessage Warn(string message, string? propertyName = null) => new(ValidationSeverity.Warning, message, propertyName);

    /// <summary>Creates an info message.</summary>
    [Pure]
    public static ValidationMessage Info(string message, string? propertyName = null) => new(ValidationSeverity.Info, message, propertyName);

    /// <summary>Creates a validation message.</summary>
    [Pure]
    public static IValidationMessage For(ValidationSeverity severity, string message, string? propertyName = null) => severity switch
    {
        ValidationSeverity.None => None,
        ValidationSeverity.Info => Info(message, propertyName),
        ValidationSeverity.Warning => Warn(message, propertyName),
        _ => Error(message, propertyName),
    };
}
