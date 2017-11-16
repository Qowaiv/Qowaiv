using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.ComponentModel.Rules
{
    /// <summary>Represents a <see cref="List{T}"/> of <see cref="IValidationRule"/>s.</summary>
    public class ValidationRules : List<IValidationRule>
    {
        /// <summary>Creates a new instance of <see cref="ValidationRules"/>.</summary>
        public ValidationRules(IEnumerable<IValidationRule> rules) : base(rules) { }

        /// <summary>Validates all <see cref="IValidationRule"/>s.</summary>
        /// <param name="validationContext">
        /// The <see cref="ValidationContext"/> in most cases provided via the
        /// <see cref="AnnotatedModelValidator"/>.
        /// </param>
        /// <returns>
        /// All <see cref="ValidationResult"/> that are not equal to <see cref="ValidationMessage.None"/>.
        /// </returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => this.Select(rule => rule.Validate(validationContext)).GetWithSeverity();

        /// <summary>Validates the specified <see cref="IValidationRule"/>s.</summary>
        /// <param name="validationContext">
        /// The <see cref="ValidationContext"/> in most cases provided via the
        /// <see cref="AnnotatedModelValidator"/>.
        /// </param>
        /// <param name="rules">
        /// The <see cref="IValidationRule"/>s to validate.
        /// </param>
        /// <returns>
        /// All <see cref="ValidationResult"/> that are not equal to <see cref="ValidationMessage.None"/>.
        /// </returns>
        public static IEnumerable<ValidationResult> Validate(ValidationContext validationContext, params IValidationRule[] rules)
        {
            var collection = new ValidationRules(rules);
            return collection.Validate(validationContext);
        }
    }
}
