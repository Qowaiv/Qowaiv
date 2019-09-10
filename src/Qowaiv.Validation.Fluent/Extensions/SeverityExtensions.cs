using Qowaiv;
using Qowaiv.Validation.Abstractions;

namespace FluentValidation
{
    /// <summary>Extensions on <see cref="Severity"/>.</summary>
    public static class SeverityExtensions
    {
        /// <summary>Converts <see cref="Severity"/> to <see cref="ValidationSeverity"/>.</summary>
        public static ValidationSeverity ToValidationSeverity(this Severity severity)
        {
            Guard.DefinedEnum(severity, nameof(severity));

            switch (severity)
            {
                case Severity.Error: return ValidationSeverity.Error;
                case Severity.Warning: return ValidationSeverity.Warning;
                case Severity.Info: return ValidationSeverity.Info;
                default: return ValidationSeverity.None;
            }
        }
    }
}
