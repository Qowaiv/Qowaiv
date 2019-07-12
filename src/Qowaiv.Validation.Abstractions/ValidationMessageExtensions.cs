using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.Validation.Abstractions
{
    /// <summary>Extensions on <see cref="IValidationMessage"/>.</summary>
    public static class ValidationMessageExtensions
    {
        /// <summary>Gets all messages excluding <see cref="ValidationSeverity.None"/>.</summary>
        public static IEnumerable<IValidationMessage> GetWithSeverity(this IEnumerable<IValidationMessage> validationResults)
        {
            Guard.NotNull(validationResults, nameof(validationResults));
            return validationResults.Where(message => message.Severity > ValidationSeverity.None);
        }

        /// <summary>Gets all messages with <see cref="ValidationSeverity.Error"/>.</summary>
        public static IEnumerable<IValidationMessage> GetErrors(this IEnumerable<IValidationMessage> validationResults)
        {
            Guard.NotNull(validationResults, nameof(validationResults));
            return validationResults.Where(message => message.Severity == ValidationSeverity.Error);
        }

        /// <summary>Gets all messages with <see cref="ValidationSeverity.Warning"/>.</summary>
        public static IEnumerable<IValidationMessage> GetWarnings(this IEnumerable<IValidationMessage> validationResults)
        {
            Guard.NotNull(validationResults, nameof(validationResults));
            return validationResults.Where(message => message.Severity == ValidationSeverity.Warning);
        }

        /// <summary>Gets all messages with <see cref="ValidationSeverity.Info"/>.</summary>
        public static IEnumerable<IValidationMessage> GetInfos(this IEnumerable<IValidationMessage> validationResults)
        {
            Guard.NotNull(validationResults, nameof(validationResults));
            return validationResults.Where(message => message.Severity == ValidationSeverity.Info);
        }
    }
}
