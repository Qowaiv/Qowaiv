using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.Rules
{
    /// <summary>Represents a validation rule defined by a delegate expression.</summary>
    /// <remarks>
    /// Used by <see cref="ValidationRule.For(Func{ValidationContext, bool?}, Func{string}, string[])"/>.
    /// </remarks>
    internal class ValidationExpression: ValidationRule
    {
        /// <summary>Creates a new instance of a <see cref="ValidationExpression"/>.</summary>
        public ValidationExpression(Func<ValidationContext, bool?> isValid, Func<string> message, params string[] propertyNames):
            base(propertyNames)
        {
            Expression = Guard.NotNull(isValid, nameof(isValid));
            ErrorMessageString = Guard.NotNull(message, nameof(message));
        }

        /// <summary>The expression to execute.</summary>
        public Func<ValidationContext, bool?> Expression { get; }
        
        /// <summary>Gets the error message.</summary>
        protected override Func<string> ErrorMessageString { get; }

        /// <summary>Returns true if the expression did not return false.</summary>
        protected override bool IsValid(ValidationContext validationContext) => Expression(validationContext) != false;
    }
}
