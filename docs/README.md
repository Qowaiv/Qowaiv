# Qowaiv

## Domain-driven design bottom up
Qowaiv is a (Single) Value Object library. It aims to model reusable (Single)
Value Objects that can be used in a wide variety of modeling scenarios, both
inside and outside a Domain-driven context.

### (Single) Value Objects
A **Value Object** is an immutable type that is distinguishable only by the
state of its properties. A **Single Value Object** (SVO) is a Value Object that
can be represented by a single scalar/primitive type.

### Primitive Obsession
**Primitive Obsession** is when the code relies too much on primitives. This is
seen as bad design, as it leads to error-prone, cluttered code. Using SVO's
instead, prevents this.

### Struct v.s. Class
All Qowaiv SVO's have been created as a `struct`, not as a `class`. The reason for
doing this, is that for primitive like SVO's they should behave similar to the
known primitives, like `double`, `int`, `DateTime`, `Guid`, etcetera.

A consequence of this choice is that SVO's can not be `null`, and that all
default initializations have a meaningful value. That can be `Empty`, `Zero`
or what suits the SVO best.

### Support
Multiple scenarios are supported:
* Parsing
* Formatting (including `ICustomFormatter`)
* Validation
* Serialization (JSON, XML)
* Domain-specific logic
* Explicit and implicit casting

#### Multi-language
Qowaiv ships with localized labels for the following languages. English
(American) is the default (neutral) language.

| Language            | Culture |
|---------------------|---------|
| English (American)  | `en`    |
| English (British)   | `en-GB` |
| Arabic              | `ar`    |
| Chinese             | `zh`    |
| Chinese (Hong Kong) | `zh-HK` |
| Chinese (Taiwan)    | `zh-TW` |
| Dutch               | `nl`    |
| French              | `fr`    |
| German              | `de`    |
| Italian             | `it`    |
| Japanese            | `ja`    |
| Portuguese          | `pt`    |
| Russian             | `ru`    |
| Spanish             | `es`    |

Note that exceptions other than the `FormatException` are not translated as
they are considered part of system, rather then a specific domain.

## Building your own
If you need a Single Value Object that is not provided by Qowaiv you can build
your own. How to do that can be read [here](../README.Custom.SVO.md).

## Qowaiv types
* [Date](svo/Date.md)
* [Date span](svo/DateSpan.md)
* [Email address](svo/EmailAddress.md)
* [Stream size](svo/StreamSize.md)
* [House number](svo/HouseNumber.md)
* [Local Date Time](svo/LocalDateTime.md)
* [Month](svo/Month.md)
* [Month span](svo/MonthSpan.md)
* [Percentage](svo/Percentage.md)
* [Postal code](svo/PostalCode.md)
* [Sex](svo/Sex.md)
* [Week date](svo/WeekDate.md)
* [Year](svo/Year.md)
* [Year-month](svo/YearMonth.md)
* [Year span](svo/YearSpan.md)
* [Yes-no](svo/YesNo.md)

## Qowaiv chemistry types
* [CAS Registry Number](svo/CasRegistryNumber.md)

## Qowaiv financial types
* [Amount](svo/Amount.md)
* [Business Identifier Code (BIC)](svo/BusinessIdentifierCode.md)
* [Currency](svo/Currency.md)
* [International Bank Account Number (IBAN)](svo/InternationalBankAccountNumber.md)
* [Money](svo/Money.md)

## Qowaiv globalization types
* [Country](svo/Country.md)
* [CultureInfoScope](svo/CultureInfoScope.md)

## Qowaiv mathematical types
* [Fraction](svo/Fraction.md)

## Qowaiv identifiers

### Strongly-typed identifiers
Primitive Obsession is a common issue when dealing with identifiers. It is quite
common to provide two or even more identifiers (of different identities) to a
method, which can lead to nasty bugs.

More info about those generated strongly-typed IDs can be found [here](../src/Qowaiv.CodeGeneration.SingleValueObjects/README.md).

* [UUID aka GUID](svo/Uuid.md)

## Qowaiv security types
* [Secret](svo/Secret.md)
* [Cryptographic Seed](svo/CryptographicSeed.md)

## Qowaiv statistical types
* [Elo](svo/Elo.md)

## Qowaiv sustainability types
* [(EU) Energy label](svo/EnergyLabel.md)

## Qowaiv web types
* [Internet media type](svo/InternetMediaType.md)

## Qowaiv complex types
* [Wildcard pattern](svo/WildcardPattern.md)

## Extensions on .NET built-in types
For types that are shipped with the .NET standard library that meet (most) SVO
criteria, Qowaiv can offer some extensions to improve usage.

* [System.DateOnly](svo/system/DateOnly.md)
* [System.DateTime](svo/system/DateTime.md)
* [System.Globalization.CultureInfo](svo/system/CultureInfo.md)
* [System.Net.Http.HttpMethod](svo/system/HttpMethod.md)

## Qowaiv helpers
* [DecimalMath](DecimalMath.md)

## Serialization
* [JSON](json/README.md)

### XML
.NET supports XML Serialization out-of-the-box. All SVOs implement `IXmlSerialization`
with the same approach:
``` C#
XmlSchema IXmlSerializable.GetSchema() => null;

void IXmlSerializable.ReadXml(XmlReader reader)
{
    var s = reader.ReadElementString();
    var val = Parse(s, CultureInfo.InvariantCulture);
    // This is a work-around to keep the structs read-only.
    System.Runtime.CompilerServices.Unsafe.AsRef(this) = val;
}

void IXmlSerializable.WriteXml(XmlWriter writer)
{
    writer.WriteString(ToString(SerializableFormat, CultureInfo.InvariantCulture));
}
```

## Qowaiv SVO options

### Sortable
SVOs support sorting. So, LINQ expressions like `OrderBy()` and `OrderByDescending()`
work out-of-the-box, just like `Array.Sort()`, and `List.Sort()`. However, the
comparison operators (`<`, `>`, `<=`, `>=`) only make sense for a subset of those,
and are not implemented on all.

Therefore

``` CSharp
#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
```

is fine for types that are sortable via IComparable (in most cases).

### Debugger display
During debugging sessions, by default, the IDE shows the result of `ToString()`
on a watch. Although `ToString()` is overridden for all Qowaiv Single Value 
Objects, for debugging a special debugger display is provided too, using a 
debugger display attribute.

The debugger display attribute refers to a (private) property with the name 
"DebuggerDisplay", which represents the Single Value Object as string. If 
supported, formatted, and in case of an Empty or Unknown value with a 
notification of that too. The outcome of the DebuggerDisplay is tested in the 
UnitTests.

Because the rendering of debugger display is handled based on the development 
environment, and methods as debugger display are not supported by VB.NET, the 
debugger display attribute refers to a property instead.

## Qowaiv Formatting
Formatting is an important part of the functionality in Qowaiv. All SVOs 
implement IFormattable, and have custom formatting. For details, see the 
different remarks at the `ToString(string, IFormatProvider)`.

All SVOs support the `ICustomFormatter` interface; if the `IFormatProvider`
returns an `ICustomFormatter` on the `GetFormat(Type? type)` call and the
custom formatter actually returns a non-null string, that formatted result is
returned by `ToString(string, IFormatProvider)`.

### Formatting arguments
The formatting arguments object, is a container object (struct) of the format 
and the format provider, the two arguments required for the `System.IFormattable`
`ToString()` method.

### Formatting arguments collection
This collection of formatting arguments stores them based on a type to apply 
on. On top of that, it has a Format() method, that is an extended implementation 
of string.Format(). The difference between these two methods is, that - when no 
custom format is supplied at the format string - string.Format() the default 
formatting of the object is used, where FormattingArgumentsCollection.Format() 
uses the default specified at the formatting collection of a type (if available).

### Threading
Because there are scenarios where you want to set typical values as a country 
or a currency for the context of the current thread (like the culture info) 
there is a possibility to add these to the Qowaiv.Threading.ThreadDomain.

These values can be configured (in the application settings) or can be created with
a creator function that can be registered. If not specified otherwise the current 
country will be created (if possible) based on the current culture.

## Qowaiv clock
* [Clock](Clock.md)

# Qowaiv Diagnostics Contracts
This package contains attributes to define (expected) behavior on code
[(..)](../src/Qowaiv.Diagnostics.Contracts/README.md)
