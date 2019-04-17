using Qowaiv.ComponentModel.Messages;
using Qowaiv.Globalization;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.Rules.Globalization
{
    /// <summary>Rule that can validate a <see cref="PostalCode"/> 
    /// being valid for a specific <see cref="Country"/>.
    /// </summary>
    public static class PostalCodeRule
    {
        /// <summary>Returns <see cref="ValidationMessage.None"/> if the <see cref="PostalCode"/> 
        /// is valid for a specific <see cref="Country"/>, otherwise an error message.
        /// </summary>
        public static ValidationResult ValidFor(PostalCode postalCode, Country country, string postalCodeName, string errorMessage = null)
        {
            Guard.NotNullOrEmpty(postalCodeName, nameof(postalCodeName));

            if (postalCode.IsEmptyOrUnknown() || country.IsEmptyOrUnknown() || postalCode.IsValid(country))
            {
                return ValidationMessage.None;
            }
            var message = string.Format(errorMessage ?? QowaivComponentModelMessages.PostalCodeValidator_ErrorMessage, postalCode, country.DisplayName);
            return ValidationMessage.Error(message, postalCodeName);
        }


        /// <summary>Returns <see cref="ValidationMessage.None"/> if the <see cref="PostalCode"/> 
        /// is not null or empty or if the specific <see cref="Country"/> does not have postal codes, otherwise an error message.
        /// </summary>
        public static ValidationResult RequiredFor(PostalCode postalCode, Country country, string postalCodeName, string errorMessage = null)
        {
            Guard.NotNullOrEmpty(postalCodeName, nameof(postalCodeName));

            if (PostalCodeCountryInfo.GetInstance(country).HasPostalCode && postalCode.IsEmptyOrUnknown())
            {
                var message = string.Format(errorMessage ?? QowaivComponentModelMessages.PostalCodeValidator_ErrorMessage, postalCode, country.DisplayName);
                return ValidationMessage.Error(message, postalCodeName);
            }
            return ValidationMessage.None;
            
        }
    }
}
