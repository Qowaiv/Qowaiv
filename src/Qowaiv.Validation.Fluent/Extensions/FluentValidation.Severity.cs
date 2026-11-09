namespace FluentValidation;

/// <summary>Extensions on <see cref="Severity"/>.</summary>
public static class QowaivSeverityExtensions
{
    extension(Severity severity)
    {
        /// <summary>Converts <see cref="Severity"/> to <see cref="ValidationSeverity"/>.</summary>
        [Pure]
        public ValidationSeverity ToValidationSeverity() => Guard.DefinedEnum(severity) switch
        {
            Severity.Error => ValidationSeverity.Error,
            Severity.Warning => ValidationSeverity.Warning,
            Severity.Info => ValidationSeverity.Info,
            _ => ValidationSeverity.None,
        };
    }
}
