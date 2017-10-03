using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.Rules
{
	/// <summary>The rule equivalent of the <see cref="ValidationAttribute"/>.</summary>
	/// <remarks>
	/// Where the <see cref="ValidationAttribute"/> should apply on isolated
	/// properties, an <see cref="IValidationRule"/> should be used to define
	/// constraints that imply multiple properties.
	/// </remarks>
	public interface IValidationRule
	{
		/// <summary>Validates the rule.</summary>
		/// <param name="validationContext">
		/// The <see cref="ValidationContext"/> in most cases provided via the
		/// <see cref="AnnotatedModelValidator"/>.
		/// </param>
		/// <returns>
		/// <see cref="ValidationMessage.None"/> when the rule is valid, 
		/// otherwise an descriptive <see cref="ValidationErrorMessage"/>.
		/// </returns>
		ValidationResult Validate(ValidationContext validationContext);
	}
}
