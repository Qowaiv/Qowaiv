using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.ComponentModel.Rules.Globalization;
using Qowaiv.DomainModel;
using Qowaiv.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.Xample
{
    public sealed class Address : ValueObject<Address>
    {
        public Address(string street, HouseNumber houseNumber, PostalCode postalCode, Country country)
        {
            Street = street;
            HouseNumber = houseNumber;
            PostalCode = postalCode;
            Country = country;
            Validate();
        }

        [Mandatory]
        public string Street { get; }

        [Mandatory, Display(Name = "House number")]
        public HouseNumber HouseNumber { get; }

        [Display(Name = "Postal code")]
        public PostalCode PostalCode { get; }

        [Mandatory]
        public Country Country { get; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return PostalCodeRule.ValidFor(PostalCode, Country, nameof(PostalCode));
            yield return PostalCodeRule.RequiredFor(PostalCode, Country, nameof(PostalCode));
        }

        /// <inheritdoc />
        public override bool Equals(Address other)
        {
            if (AreSame(other)) { return true; }
            return NotNull(other)
                && Street == other.Street
                && HouseNumber == other.HouseNumber
                && PostalCode == other.PostalCode
                && Country == other.Country
                ;
        }

        /// <inheritdoc />
        protected override int Hash()
        {
            return QowaivHash.HashObject(Street)
                ^ QowaivHash.Hash(HouseNumber)
                ^ QowaivHash.Hash(PostalCode, 7)
                ^ QowaivHash.Hash(Country, 13)
                ;
        }
    }
}
