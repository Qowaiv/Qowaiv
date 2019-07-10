using FluentValidation.Validators;

namespace Qowaiv.Validation.Fluent.Validators
{
    public class NotUnknownValidator : PropertyValidator
    {
        private readonly object _unknownValueForType;

        public NotUnknownValidator(object unknownValueForType)
            : base(nameof(NotUnknownValidator), typeof(QowaivValidationFluentMessages))
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
