using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Rules;
using Qowaiv.ComponentModel.Rules.Globalization;
using Qowaiv.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.UnitTests.Validation.Models
{
    public class PostalCodeModel : IValidatableObject
    {
        [Mandatory]
        public Country Country { get; set; }

        [Mandatory]
        public PostalCode PostalCode { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return PostalCodeRule.ValidFor(PostalCode, Country, nameof(PostalCode));
                
            if(validationContext.GetSevice<AddressService>()?.PostalCodeExists(PostalCode) == false)
            {
                yield return ValidationMessage.Error("Postal code does not exist.", nameof(PostalCode));
            }
        }
    }
    public class AddressService
    {
        public bool PostalCodeExists(PostalCode code) => code == PostalCode.Parse("2629 JD");
    }
}
