# Qowaiv Domain Model

## Complex Value Objects
Complex Value Objects - opposed to Single Value Objects (SVO's), those who can
be represented by a single scalar - are represented by multiple (immutable)
scalars. Qowaiv provides a generic base class helping to achieve that.

``` C#
public sealed class Address : SingleValueObject<Address>
{
    public Address(string street, HouseNumber number, PostalCode code, Country country)
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
}
```
