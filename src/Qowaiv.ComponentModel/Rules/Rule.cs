using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Validation;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.ComponentModel.Rules
{
    /// <summary>Rule factory class.</summary>
    public static class Rule
    {
        /// <summary>Validates if a property meets the required conditions, when the depending required condition is met.</summary>
        /// <param name="validationContext">
        /// The validation context to validate the condtions.
        /// </param>
        /// <param name="requiredCondition">
        /// The condition that makes the property conditional required.
        /// </param>
        /// <param name="propertyName">
        /// The name of the property that is conditional required.
        /// </param>
        /// <param name="requiredAttribute">
        /// Optional required attribute. If not provided, a default <see cref="MandatoryAttribute"/> is used.
        /// </param>
        /// <returns>
        /// <see cref="ValidationMessage.None"/> if the dependent condition is not met.
        /// Else, the <see cref="ValidationResult"/> returned by the <see cref="RequiredAttribute"/>.
        /// </returns>
        public static ValidationResult ConditionalRequired(ValidationContext validationContext, bool requiredCondition, string propertyName, RequiredAttribute requiredAttribute = null)
        {
            Guard.NotNull(validationContext, nameof(validationContext));

            if (!requiredCondition)
            {
                return ValidationMessage.None;
            }
            var annotated = AnnotatedModel.Get(validationContext.ObjectType);
            var prop = annotated.Properties.FirstOrDefault(p => p.Name == propertyName);
            var value = prop.GetValue(validationContext.ObjectInstance);
            var subContext = validationContext.ForProperty(prop);
            var attr = requiredAttribute ?? mandatoryAttribute;
            return attr.GetValidationResult(value, subContext);
        }
        private static readonly MandatoryAttribute mandatoryAttribute = new MandatoryAttribute();
    }
}
