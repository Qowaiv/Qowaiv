using Qowaiv.Validation.Abstractions;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Represents a <see cref="ValidationResult"/> as a <see cref="IValidationMessage"/>.</summary>
[Inheritable]
[Serializable]
public class ValidationMessage : ValidationResult, IValidationMessage
{
    /// <summary>Initializes a new instance of the <see cref="ValidationMessage"/> class.</summary>
    internal ValidationMessage(ValidationSeverity severity, string? message, IEnumerable<string>? memberNames)
        : base(message, memberNames ?? [])
        => Severity = severity;

    /// <inheritdoc />
    public ValidationSeverity Severity { get; }

    /// <inheritdoc />
    public string? PropertyName => MemberNames.FirstOrDefault();

    /// <inheritdoc />
    public string? Message => ErrorMessage;

    /// <summary>Represents a validation message with severity <see cref="ValidationSeverity.None"/>.</summary>
    /// <remarks>
    /// Other tooling can not work properly with <see cref="ValidationMessage"/>s
    /// that try to represent <see cref="ValidationResult.Success"/>.
    /// </remarks>
    public static readonly ValidationMessage? None = null;

    /// <summary>Creates an error message.</summary>
    [Pure]
    public static ValidationMessage Error(string? message, params string[] memberNames) => new(ValidationSeverity.Error, message, memberNames);

    /// <summary>Creates an error message.</summary>
    [Pure]
    [OverloadResolutionPriority(1)]
    public static ValidationMessage Error(string? message, params IEnumerable<string> memberNames) => new(ValidationSeverity.Error, message, memberNames);

    /// <summary>Creates a warning message.</summary>
    [Pure]
    public static ValidationMessage Warn(string? message, params string[] memberNames) => new(ValidationSeverity.Warning, message, memberNames);

    /// <summary>Creates a warning message.</summary>
    [Pure]
    [OverloadResolutionPriority(1)]
    public static ValidationMessage Warn(string? message, params IEnumerable<string> memberNames) => new(ValidationSeverity.Warning, message, memberNames);

    /// <summary>Creates an info message.</summary>
    [Pure]
    public static ValidationMessage Info(string? message, params string[] memberNames) => new(ValidationSeverity.Info, message, memberNames);

    /// <summary>Creates an info message.</summary>
    [Pure]
    [OverloadResolutionPriority(1)]
    public static ValidationMessage Info(string? message, params IEnumerable<string> memberNames) => new(ValidationSeverity.Info, message, memberNames);

    /// <summary>Creates a validation message for a validation result.</summary>
    /// <param name="validationResult">
    /// The validation result to convert.
    /// </param>
    [Pure]
    public static ValidationMessage? For(ValidationResult? validationResult) => validationResult switch
    {
        ValidationMessage message => message,
        null => None,
        var r when r == Success => None,
        _ => Error(validationResult.ErrorMessage, [.. validationResult.MemberNames]),
    };

    /// <summary>Creates a validation message.</summary>
    [Pure]
    public static IValidationMessage? For(ValidationSeverity severity, string message, params string[] memberNames) => severity switch
    {
        ValidationSeverity.None => None,
        ValidationSeverity.Info => Info(message, memberNames),
        ValidationSeverity.Warning => Warn(message, memberNames),
        _ => Error(message, memberNames),
    };
}
