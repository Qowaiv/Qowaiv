using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.Globalization;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public sealed class AddressValueObject : ValueObject<AddressValueObject>
    {
        public AddressValueObject(string street, HouseNumber number, PostalCode code, Country country)
        {
            Street = street;
            HouseNumber = number;
            PostalCode = code;
            Country = country;
            Validate();
        }

        [Mandatory]
        public string Street { get; }
        [Mandatory]
        public HouseNumber HouseNumber { get; }
        public PostalCode PostalCode { get; }
        [Mandatory]
        public Country Country { get; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Country == Country.NL && PostalCode.IsEmptyOrUnknown())
            {
                yield return ValidationMessage.Error("Postal code is required.", nameof(PostalCode));
            }
            if (!Country.IsEmptyOrUnknown() && !PostalCode.IsEmptyOrUnknown() && !PostalCode.IsValid(Country))
            {
                yield return ValidationMessage.Error($"Postal code is not valid for {Country:f}.", nameof(PostalCode));
            }
        }

        public override bool Equals(AddressValueObject other)
        {
            if (AreSame(other))
            {
                return true;
            }
            return NotNull(other)
                && Street == other.Street
                && HouseNumber == other.HouseNumber
                && PostalCode == other.PostalCode
                && Country == other.Country
                ;
        }

        protected override int Hash()
        {
            return QowaivHash.HashObject(Street)
                ^ QowaivHash.Hash(HouseNumber, 3)
                ^ QowaivHash.Hash(PostalCode, 5)
                ^ QowaivHash.Hash(Country, 13)
                ;
        }
    }
}
