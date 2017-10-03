using Qowaiv.ComponentModel.Messages;
using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.Rules
{
    internal class ValidationExpression: IValidationRule
    {
        public ValidationExpression(Func<ValidationContext, bool?> isValid, Func<string> message, params string[] propertyNames)
        {
            IsValid = Guard.NotNull(isValid, nameof(isValid));
            Message = Guard.NotNull(message, nameof(message));
            PropertyNames = Guard.NotNull(propertyNames, nameof(propertyNames));
        }

        public Func<ValidationContext, bool?> IsValid { get; }
        public Func<string> Message { get; }
        public string[] PropertyNames { get; }

        public ValidationResult Validate(ValidationContext validationContext)
        {
            if (IsValid(validationContext) != false)
            {
                return ValidationMessage.None;
            }
            return ValidationMessage.Error(Message(), PropertyNames);
        }
    }
}
