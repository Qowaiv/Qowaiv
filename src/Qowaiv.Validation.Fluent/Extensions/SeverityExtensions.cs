using Qowaiv;
using Qowaiv.Validation.Abstractions;

namespace FluentValidation
{
    public static class SeverityExtensions
    {
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
