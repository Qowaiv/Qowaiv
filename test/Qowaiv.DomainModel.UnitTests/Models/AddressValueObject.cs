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
        }

        public string Street { get; }
        public HouseNumber HouseNumber { get; }
        public PostalCode PostalCode { get; }
        public Country Country { get; }

        public override bool Equals(AddressValueObject other)
        {
            if (AreSame(other)) { return true; }

            return NotNull(other)
                && Street == other.Street
                && HouseNumber == other.HouseNumber
                && PostalCode == other.PostalCode
                && Country == other.Country;
        }

        protected override int Hash()
        {
            return QowaivHash.HashObject(Street)
                ^ QowaivHash.Hash(HouseNumber, 3)
                ^ QowaivHash.Hash(PostalCode, 5)
                ^ QowaivHash.Hash(Country, 13);
        }

        public override string ToString() => $"{Street} {HouseNumber}, {PostalCode.ToString(Country.Name)}, {Country.DisplayName}";
    }
}
