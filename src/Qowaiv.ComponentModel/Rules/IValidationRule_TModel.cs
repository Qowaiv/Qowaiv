using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.Rules
{
    /// <summary>The rule equivalent of the <see cref="ValidationAttribute"/>.</summary>
    /// <remarks>
    /// Where the <see cref="ValidationAttribute"/> should apply on isolated
    /// properties, an <see cref="IValidationRule"/> should be used to define
    /// constraints that imply multiple properties.
    /// </remarks>
    public interface IValidationRule<TModel>
        where TModel: class
    {
        /// <summary>Validates the rule.</summary>
        /// <param name="validationContext">
        /// The context to validate.
        /// </param>
        IEnumerable<ValidationResult> Validate(ValidationContext<TModel> validationContext);
    }
}
