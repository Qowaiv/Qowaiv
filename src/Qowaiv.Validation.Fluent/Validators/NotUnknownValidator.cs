using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Qowaiv.Validation.Fluent.Validators
{
    /// <summary><see cref="PropertyValidator"/> that validates that a property is not <see cref="Unknown"/>.</summary>
    public class NotUnknownValidator : PropertyValidator
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
            Guard.NotNull(context, nameof(context));

            return !_unknownValueForType.Equals(context.PropertyValue);
        }
    }
}
