using Qowaiv.ComponentModel.Messages;
using Qowaiv.Globalization;
using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.Rules.Globalization
{
    /// <summary>Rule that can validate a <see cref="Qowaiv.PostalCode"/> 
    /// being valid for a specific <see cref="Qowaiv.Globalization.Country"/>.
    /// </summary>
    public partial class PostalCodeRule : ValidationRule
    {
        /// <summary>Creates a new instance of a <see cref="PostalCodeRule"/>.</summary>
        /// <param name="postalCode">
        /// The postal code to validate.
        /// </param>
        /// <param name="country">
        /// The country to validate it for.
        /// </param>
        /// <param name="postalCodeName">
        /// The name of the <see cref="Qowaiv.PostalCode"/> property in the model.
        /// </param>
        /// <param name="countryName">
        /// The name of the <see cref="Qowaiv.Globalization.Country"/> property in the model.
        /// </param>
        [Obsolete("Use PostalCodeRule<TModel> instead.")]
        public PostalCodeRule(PostalCode postalCode, Country country, string postalCodeName, string countryName)
            : base(postalCodeName, countryName)
        {
            PostalCode = postalCode;
            Country = country;
        }

        /// <summary>Gets the involved postal code.</summary>
        public PostalCode PostalCode { get; }
        /// <summary>Gets the involved country.</summary>
        public Country Country { get; }

        /// <summary>Returns true if postal code is valid (or if the postal code or country is empty or unknown).</summary>
        protected override bool IsValid(ValidationContext validationContext)
        {
            return PostalCode.IsEmptyOrUnknown() || Country.IsEmptyOrUnknown() || PostalCode.IsValid(Country);
        }

        /// <summary>Gets the error message.</summary>
        protected override Func<string> ErrorMessageString => () => string.Format(QowaivComponentModelMessages.PostalCodeValidator_ErrorMessage, PostalCode, Country.DisplayName);

        /// <summary>Returns <see cref="ValidationMessage.None"/> if the <see cref="PostalCode"/> 
        /// is valid for a specific <see cref="Country"/>, otherwise false.
        /// </summary>
        [Obsolete("Use PostalCodeRule.For<TModel> instead.")]
        public static ValidationResult IsValid(PostalCode postalCode, Country country, string postalCodeName, string countryName)
        {
            var rule = new PostalCodeRule(postalCode, country, postalCodeName, countryName);
            return rule.Validate(null);
        }
    }
}
