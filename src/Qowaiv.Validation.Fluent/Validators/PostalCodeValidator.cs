using FluentValidation.Resources;
using FluentValidation.Validators;
using Qowaiv.Globalization;
using System;

namespace Qowaiv.Validation.Fluent.Validators
{
    /// <summary><see cref="PropertyValidator"/> that validates that a postal code is valid for a specific country.</summary>
    public class PostalCodeValidator : PropertyValidator
    {
        private readonly Func<object, Country> _country;

        /// <summary>Creates a new instance of <see cref="PostalCodeValidator"/>.</summary>
        public PostalCodeValidator(Country country) : this((obj) => country) { }

        /// <summary>Creates a new instance of <see cref="PostalCodeValidator"/>.</summary>
        public PostalCodeValidator(Func<object, Country> country) 
            : base(new LazyStringSource(context => QowaivValidationFluentMessages.PostalCodeValidForCountry))
        {
            _country = Guard.NotNull(country, nameof(country));
        }

        /// <inheritdoc />
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var postalCode = (PostalCode)context.PropertyValue;
            var country = _country(context.Instance);

            if(country.IsEmptyOrUnknown() 
                || postalCode.IsEmptyOrUnknown()
                || postalCode.IsValid(country))
            {
                return true;
            }
            context.MessageFormatter
                .AppendArgument(nameof(Country), country.DisplayName)
                .AppendArgument("Value", postalCode);

            return false;
        }
    }
}
