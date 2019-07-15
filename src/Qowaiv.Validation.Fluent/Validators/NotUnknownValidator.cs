using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Qowaiv.Validation.Fluent.Validators
{
    /// <summary><see cref="PropertyValidator"/> that validates that a property is not <see cref="Unknown"/>.</summary>
    internal class NotUnknownValidator : PropertyValidator
    {
        private readonly object _unknownValueForType;

        /// <summary>Creates a new instance of a <see cref="NotUnknownValidator"/>.</summary>
        public NotUnknownValidator(object unknownValueForType)
            : base(new LazyStringSource(context => QowaivValidationFluentMessages.NotUnknown))
        {
            _unknownValueForType = unknownValueForType;
        }

        /// <inheritdoc />
        protected override bool IsValid(PropertyValidatorContext context)
        {
            return context.PropertyValue is null 
                || !Equals(_unknownValueForType, context.PropertyValue);
        }
    }
}
