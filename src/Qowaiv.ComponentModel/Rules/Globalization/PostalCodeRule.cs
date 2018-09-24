using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.Globalization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;

namespace Qowaiv.ComponentModel.Rules.Globalization
{
    public partial class PostalCodeRule
    {
        /// <summary>Creates a postal code rule for the specified model.</summary>
        /// <typeparam name="TModel">
        /// The model to validate.
        /// </typeparam>
        /// <param name="getPostalCode">
        /// The expression to retrieve the postal code.
        /// </param>
        /// <param name="getCountry">
        /// The expression to retrieve the country to validate the postal code for.
        /// </param>
        public static PostalCodeRule<TModel> For<TModel>(
            Expression<Func<TModel, PostalCode>> getPostalCode, 
            Expression<Func<TModel, Country>> getCountry)
            where TModel: class
        {
            Guard.NotNull(getPostalCode, nameof(getPostalCode));
            Guard.NotNull(getCountry, nameof(getCountry));

            var postalCodeName = ExpressionRule.GetMemberName(getPostalCode);

            return new PostalCodeRule<TModel>(getPostalCode.Compile(), getCountry.Compile(), postalCodeName);
        }
    }

    /// <summary>Rule that can validate a <see cref="Qowaiv.PostalCode"/> 
    /// being valid for a specific <see cref="Qowaiv.Globalization.Country"/>.
    /// </summary>
    public class PostalCodeRule<TModel> : ValidationRule<TModel>
        where TModel : class
    {
        /// <summary>Creates a new instance of the rule.</summary>
        internal PostalCodeRule(Func<TModel, PostalCode> postalCode, Func<TModel, Country> country, string postalCodePropertyName)
            : base(postalCodePropertyName)
        {
            getPostalCode = postalCode;
            getCountry = country;
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext<TModel> validationContext)
        {
            Guard.NotNull(validationContext, nameof(validationContext));

            var postalCode = getPostalCode(validationContext.Model);
            var country = getCountry(validationContext.Model);

            if (!postalCode.IsEmptyOrUnknown() && !postalCode.IsValid(country))
            {
                yield return ValidationMessage.Error(string.Format(CultureInfo.CurrentCulture, ErrorMessageString(), postalCode, country), new[] { PropertyNames[0] });
            }
        }

        /// <summary>Gets the error message.</summary>
        protected override Func<string> ErrorMessageString => () => QowaivComponentModelMessages.PostalCodeValidator_ErrorMessage;

        private readonly Func<TModel, PostalCode> getPostalCode;
        private readonly Func<TModel, Country> getCountry;
    }
}
