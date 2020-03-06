![Qowaiv](https://github.com/Qowaiv/Qowaiv/blob/master/design/qowaiv-logo_linkedin_100x060.jpg)

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Code of Conduct](https://img.shields.io/badge/%E2%9D%A4-code%20of%20conduct-blue.svg?style=flat)](https://github.com/Qowaiv/Qowaiv/blob/master/CODE_OF_CONDUCT.md)

| version                                                                      | package                                                                     |
|------------------------------------------------------------------------------|-----------------------------------------------------------------------------|
|![v](https://img.shields.io/badge/version-5.0.1-blue.svg?cacheSeconds=3600)   |[Qowaiv](https://www.nuget.org/packages/Qowaiv/)                             |
|![v](https://img.shields.io/badge/version-5.0.0-blue.svg?cacheSeconds=3600)   |[Qowaiv.Data.SqlCient](https://www.nuget.org/packages/Qowaiv.Data.SqlClient/)|
|![v](https://img.shields.io/badge/version-3.0.0-darkblue.svg?cacheSeconds=3600)|[Qowaiv.TestTools](https://www.nuget.org/packages/Qowaiv.TestTools/)         |

# Qowaiv

## Domain-driven design bottom up
Qowaiv is a (Single) Value Object library. It aims to model reusable (Single)
Value Objects that can be used a wide variety of modeling scenarios, both
inside and outside a Domain-driven context.

Supported scenarios include parsing, formatting, validation, (de)serialization,
model binding, and domain specific logic.

# Single Value Object
A Value Object that can be represented by a single scalar.

## Technical requirements
Visual Studio VS2017.3 or higher is required. Visual Studio can be downloaded
here: [visualstudio.com/downloads](https://www.visualstudio.com/downloads/).

## Qowaiv types

### Date
Represents a date, so without hours (minutes, seconds, milliseconds).

### Date span
Represents a date span. Opposed to a `TimeSpan` its duration is (a bit) resilient;
Adding one month to a date in January result in adding a different number of days, 
than adding one month a date in March.

Date spans are particular useful in scenario's for defining (and doing calculations on)
month based periods, and ages (mostly in years and days).

``` C#
var span = new DateSpan(years: 3, months: 2, days: -4);
var age = DateSpan.Age(new Date(2017, 06, 11)); // 2Y+0M+121D on 2019-10-10
var duration = DateSpan.Subtract(new Date(2019, 06, 10), new Date(2017, 06, 11)); // 1Y+11M+30D
var date = new Date(2016, 06, 03).Add(age); // 2018-10-02
```

### Elo
Represents an Elo (rating), a method for calculating the relative skill levels of
players in competitor-versus-competitor games.

### Email address
Represents a (single) email address. Supports:
* Display names (are stripped)
* Comments (are removed)
* IP-based domains (normalized and surrounded by brackets)

Furthermore, the email address is normalized as a lowercase string, making it
case-insensitive.

``` C#
var email = EmailAddress.Parse("Test Account <mailto:TEST@qowaiv.org>");
var quoted = EmailAddress.Parse("\"Joe Smith\" email@qowaiv.org");
var ip_based = EmailAddress.Parse("test@[172.16.254.1]");

email.ToString(); // test@qowaiv.org
quoted.ToString(); // email@qowaiv.org
ip_based.IsIPBased; // true
ip_based.WithDisplayName("Jimi Hendrix"); // Jimi Hendrix <test@[172.16.254.1]>

```

### Email address collection
Represents a collection of unique email addresses, excluding the empty and unknown email address.

### Stream size
Represents the size of a file or stream.

### Gender
Represents a gender based on an ISO 5218 code.

### House number
Represents a house number in the range [1-999999999].

### Local Date Time
Explicitly marked local date time. It allows the clear distinction between local 
and UTC-based date times.

### Month
Represents a month in the range [1-12].

``` C#
Month feb = Month.Parse("February");
Month may = Month.May;
Month dec = 12;

feb.ToString("f", new CultureInfo("nl-NL")); // februari
feb.ToString("s"); // Feb
feb.ToString("M"); // 02
feb.ToString("m"); // 2
```

### Percentage
Represents a percentage. It supports parsing from per mile and per ten thousand
too. The basic thought is that `Percentage.Parse("14%")` has the same result
as `double.Parse("14%")`, which is `0.14`.

``` C#
// Creation
Percentage p = 0.0314; // implicit cast: 3.14%
var p = Percentage.Parse("3.14"); //  Parse: 3.14%;
var p = Percentage.Parse("3.14%"); // Parse: 3.14%;
var p = Percentage.Parse("31.4‰"); // Parse: 3.14%;
var p = 3.14.Percent(); // Extension on double: 3.14%;

// Casting
var p = (Percentage)0.500; // 50%
var p = (Percentage)0.33m; // 33%

// Manipulation
var p = 13.2.Percent();
p++; // 14.2%;
var total = 400;
total *= 50.Percent(); // Total = 200;
var value = 50.0;
value += 10.Percent(); // value 55;

var rounded = 17.56.Percent().Round(1); // 17.6%;

var max = Percentage.Max(1.4.Percent(), 1.8.Percent()); // 1.8%;
var min = Percentage.Min(1.7.Percent(), 1.9.Percent()); // 1.7%;
```

### Postal code
Represents a postal code. It supports validation for all countries.

### UUID aka GUID
The UUID (Universally unique identifier) aka GUID (Globally unique identifier) is an
extension on the System.Guid. It is by default represented by a 22 length string, 
instead of a 32 length string.

``` C#
var rnd = Uuid.NewUuid();
UuidVersion version = rnd.Version; // UuidVersion.Random = 4

var bytes = Encoding.ASCII.GetBytes("Qowaiv");
var md5 = Uuid.GenerateWithMD5(bytes); //   lmZO_haEOTCwGsCcbIZFFg, UUID Version: 3
var sha1 = Uuid.GenerateWithSHA1(bytes); // 39h-Y1rR51ym_t78x9h0bA, UUID Version: 5
```

#### Comparer
The UUID Comparer can sort both UUID's as GUID's, Furthermore, is support both
.NET's default way of sorting as the sorting of SQL Server, or MongoDB.

``` C#
var uuids = new List<Uuid>();
uuids.Sort(UuidComparer.SqlServer);

var uuids = new List<Uuid>();
uuids.Sort(UuidComparer.MongoDb);

var guids = new List<Guid>();
guids.Sort(UuidComparer.Default);
```

#### Sequential
As UUID's are commonly used for the clustered key of a database table. For
massive database with a lot of inserts (they go hand in hand normally) this can
be a performance issue, as by default generated UUID's are not sequential, so
the clustered index gets a lot of random inserts.

By using a sequential UUID this problem can be minimized. Obviously, if you can
fully rely on the sequential UUID generation by your database of choice, you
should consider that, but in most cases you want to generate the ID upfront.
In that case `Uuid.NewSequential()` comes handy:

``` C#
var uuid = Uuid.NewSequential(UuidComparer.SqlServer);
```

As databases might (like SQL Server does) order your UUID/GUID's differently
that .NET does, this generator does that too. Also keep in mind that this generated ID
is not perfectly sequential; first of all because it has a 0.32 nanosecond
overlap, but more seriously, as some time may elapse between the generation and
the storage in the database. Furthermore, these generated UUID's are not sequential
once mixed with the sequential generated UUID's by your database.

### Week date
Represents a week based date.

### Year
Represents a year in the range [1-9999].

### Yes-no
A Yes-no is a (bi-)polar that obviously has the values "yes" and "no". It also
has an "empty"(unset) and "unknown" value. It maps easily with a `boolean`, but
Supports all kind of formatting (and both empty and unknown) that can not be
achieved when modeling a property as `bool` instead of an `YesNo`.

## Qowaiv cryptographic types

### Seed
A seed, representing random data to encrypt and decrypt data.

## Qowaiv financial types

### Amount
Represents money without the notion of the actual currency.

### Business Identifier Code (BIC)
Represents a BIC as specified in ISO 13616.

``` C#
var bic = BusinessIdentifierCode.Parse("AEGONL2UXXX");

var business = bic.Business; // "AEGO"
var country = bic.Country; // Country.NL
var location = bic.Location; // "2U"
var branch = bic.Branch; // "XXX"
var length = bic.Length; // 11
```

### Currency
Represents a currency based on an ISO 4217 code.

### International Bank Account Number (IBAN)
Represents an IBAN as specified in ISO 13616.

``` C#
var iban = InternationalBankAccountNumber.Parse("nl20ingb0001234567");

iban.Country; // Country.NL
iban.Length; // 18
iban.ToString("F");// NL20 INGB 0001 2345 67
```

### Money
Represents the amount and the currency. Technically this is not SVO. However it
acts identically as a SVO.

``` C#
Money money = 125.34 + Currency.EUR;
var sum = (12 + Currency.EUR) + (15 + Currency.USD); // Throws CurrencyMismatchException()
var rounded = money.Round(0); // EUR 125.00
```

## Qowaiv globalization types

### Country
Represents a country based on an ISO 3166-1 code (or 3166-3 if the country does not longer exists).

## Qowaiv SQL types

### Timestamp
Represents a (MS SQL) time-stamp is a data type that exposes automatically generated
binary numbers, which are guaranteed to be unique within a database. time-stamp is
used typically as a mechanism for version-stamping table rows. The storage size is
8 bytes. See: https://technet.microsoft.com/en-us/library/aa260631%28v=sql.80%29.aspx

## Qowaiv Data

### SVO Parameter factory class
To create a (SQL) parameter with a SVO as value, use the SvoParamater factory
class. It will return SQL parameter with a converted database proof value.

## Qowaiv web types

### Internet media type
Represents an Internet media type (also known as MIME-type and content type).

## Qowaiv complex types

### Wildcard pattern
Represents a pattern to match strings, using wildcard characters ? and *. It 
also support the use of SQL wildcard characters _ and %.

## Qowaiv helpers

### Decimal round
By default, .NET support rounding of floating points (including `decimal`s).
However, for some domains this support is too limited. To overcome this, Qowaiv
has the static `DecimalRound` helper class, containing extension methods for rounding.

#### ‘Negative’ decimals
To round tenfold, hundredfold, etc. precision, a negative amount of decimals
can be specified:
``` C#
var tenfold = 1245.346m.Round(-1); // 1250m
var hundredfold = 1209m.Round(-2); // 1200m
```

#### Multiple of
Rounding to a multiple of is supported:
``` C#
var multipleOf = 123.5m.RoundToMultiple(5m); //   125.0m
var multiple25 = 123.5m.RoundToMultiple(2.5m); // 122.5m
```

#### Extra rounding methods
.NET supports rounding to even (Bankers rounding) and away from zero out-of-the-box.
Rounding methods like ceiling, floor, and truncate have limited support (0 decimals only),
and many others (to odd, half-way up, half-way down, e.o.) are missing.
By specifying the `DecimalRounding` 13 ways are supported.

``` c#
var toOdd = 23.0455m.Round(3, DecimalRounding.ToOdd); // 23.045m
var towardsZero = 23.5m.Round(DecimalRounding.TowardsZero); // 23m
var randomTie = 23.5m.Round(DecimalRounding.RandomTieBreaking); // 50% 23m, 50% 24,
```

## Model Binding
All SVO's support model binding out of the box. That is to say, when the model
binding mechanism works with a `TypeConverter`. It still may be beneficial to
have a custom model binder. Because different solutions might require different
custom model binders, and deploying them as NuGet packages would potentially
lead to a dependency hell, Qowaiv provides them as code snippets:
* [ASP.NET Core MVC ModelBinding](example/Qowaiv.AspNetCore.Mvc.ModelBinding/README.md)
* [ASP.NET (Classic) MVC ModelBinding](example/Qowaiv.Web.Mvc.ModelBinding/README.md)

## Serialization
### JSON
Serializing data using JSON is de facto the default. Qowaiv has a (naming)
based convention:

``` C#
public struct Svo
{
    public static Svo FromJson(string json);

    // When appropriate for the SVO. Example: `Percentage`.
    public static Svo FromJson(double json);

    // When appropriate for the SVO. Example: `Amount`.
    public static Svo FromJson(long json);

    // When appropriate for the SVO. Example: `YesNo`.
    public static Svo FromJson(bool json);

    // In most cases `string` is returned, but there are exceptions:
    // Amount: double ToJson();
    // StreamSize: long ToJson();
    // Year: object ToJson();
    public object /* or string, bool, int, long, double, decimal */ ToJson();
}
```
#### Implementations
There are two _out-of-the-box_ implementations that that support this convention
based contract.
* [Qowaiv.Json.Newtonsoft](https://www.nuget.org/packages/Qowaiv.Json.Newtonsoft/)  
* [Qowaiv.Text.Json.Serialization](https://www.nuget.org/packages/Qowaiv.Text.Json.Serialization/)  

#### OpenAPI Specification
The [OpenAPI Specification](https://swagger.io/docs/specification/about/)
(formerly Swagger Specification) is an API description format for REST API's.

To improve usage of your REST API's you should specify the Data Type of your
SVO's. To make this as simple as possible, Qowaiv SVO's are decorated with the
`OpenApiDataTypeAttribute`. It specifies the type, format, (regex) pattern,
and if the data type is nullable, all when applicable.

``` json
{
  "Date": {
    "description": "Full-date notation as defined by RFC 3339, section 5.6, for example, 2017-06-10.",
    "type": "string",
    "format": "date",
    "nullabe": false
  },
  "DateSpan": {
    "description": "Date span, specified in years, months and days, for example 1Y+10M+16D.",
    "type": "string",
    "format": "date-span",
    "pattern": "[+-]?[0-9]+Y[+-][0-9]+M[+-][0-9]+D",
    "nullabe": false
  },
  "EmailAddress": {
    "description": "Email notation as defined by RFC 5322, for example, svo@qowaiv.org.",
    "type": "string",
    "format": "email",
    "nullabe": true
  },
  "EmailAddressCollection": {
    "description": "Comma separated list of email addresses defined by RFC 5322.",
    "type": "string",
    "format": "email-collection",
    "nullabe": true
  },
  "Gender": {
    "description": "Gender as specified by ISO/IEC 5218.",
    "type": "string",
    "format": "gender",
    "nullabe": true,
    "enum": [
      "NotKnown",
      "Male",
      "Female",
      "NotApplicable"
    ]
  },
  "HouseNumber": {
    "description": "House number notation.",
    "type": "string",
    "format": "house-number",
    "nullabe": true
  },
  "LocalDateTime": {
    "description": "Date-time notation as defined by RFC 3339, without time zone information, for example, 2017-06-10 15:00.",
    "type": "string",
    "format": "local-date-time",
    "nullabe": false
  },
  "Month": {
    "description": "Month(-only) notation.",
    "type": "string",
    "format": "month",
    "nullabe": true,
    "enum": [
      "Jan",
      "Feb",
      "Mar",
      "Apr",
      "May",
      "Jun",
      "Jul",
      "Aug",
      "Sep",
      "Oct",
      "Nov",
      "Dec",
      "?"
    ]
  },
  "Percentage": {
    "description": "Ratio expressed as a fraction of 100 denoted using the percent sign '%', for example 13.76%.",
    "type": "string",
    "format": "percentage",
    "pattern": "-?[0-9]+(\\.[0-9])?%",
    "nullabe": false
  },
  "PostalCode": {
    "description": "Postal code notation.",
    "type": "string",
    "format": "postal-code",
    "nullabe": true
  },
  "Uuid": {
    "description": "Universally unique identifier, Base64 encoded, for example lmZO_haEOTCwGsCcbIZFFg.",
    "type": "string",
    "format": "uuid-base64",
    "nullabe": true
  },
  "WeekDate": {
    "description": "Full-date notation as defined by ISO 8601, for example, 1997-W14-6.",
    "type": "string",
    "format": "date-weekbased",
    "nullabe": false
  },
  "Year": {
    "description": "Year(-only) notation.",
    "type": "integer",
    "format": "year",
    "nullabe": true
  },
  "YesNo": {
    "description": "Yes-No notation.",
    "type": "string",
    "format": "yes-no",
    "nullabe": true,
    "enum": [
      "yes",
      "no",
      "?"
    ]
  },
  "Financial.Amount": {
    "description": "Decimal representation of a currency amount.",
    "type": "number",
    "format": "amount",
    "nullabe": false
  },
  "Financial.BusinessIdentifierCode": {
    "description": "Business Identifier Code, as defined by ISO 9362, for example, DEUTDEFF.",
    "type": "string",
    "format": "bic",
    "nullabe": true
  },
  "Financial.Currency": {
    "description": "Currency notation as defined by ISO 4217, for example, EUR.",
    "type": "string",
    "format": "currency",
    "nullabe": true
  },
  "Financial.InternationalBankAccountNumber": {
    "description": "International Bank Account Number notation as defined by ISO 13616:2007, for example, BE71096123456769.",
    "type": "string",
    "format": "iban",
    "nullabe": true
  },
  "Financial.Money": {
    "description": "Combined currency and amount notation as defined by ISO 4217, for example, EUR 12.47.",
    "type": "string",
    "format": "money",
    "pattern": "[A-Z]{3} -?[0-9]+(\\.[0-9]+)?",
    "nullabe": false
  },
  "Globalization.Country": {
    "description": "Country notation as defined by ISO 3166-1 alpha-2, for example, NL.",
    "type": "string",
    "format": "country",
    "nullabe": true
  },
  "IO.StreamSize": {
    "description": "Stream size notation (in byte).",
    "type": "integer",
    "format": "stream-size",
    "nullabe": false
  },
  "Security.Cryptography.CryptographicSeed": {
    "description": "Base64 encoded cryptographic seed.",
    "type": "string",
    "format": "cryptographic-seed",
    "nullabe": true
  },
  "Statistics.Elo": {
    "description": "Elo rating system notation.",
    "type": "number",
    "format": "elo",
    "nullabe": false
  },
  "Web.InternetMediaType": {
    "description": "Media type notation as defined by RFC 6838, for example, text/html.",
    "type": "string",
    "format": "internet-media-type",
    "nullabe": true
  }
}
```
  
#### OpenApi using Swagger 
When using [Swagger](https://swagger.io/resources/open-api/) to implement
OpenApi this could be done like below:
``` C#
/// <summary>Extensions on <see cref="SwaggerGenOptions"/>.</summary>
public static class SwaggerGenOptionsSvoExtensions
{
	/// <summary>Maps Qowaiv SVO's.</summary>
	public static SwaggerGenOptions MapSingleValueObjects(this SwaggerGenOptions options)
	{
		var attributes = OpenApiDataTypeAttribute.From(typeof(Date).Assembly);
		foreach (var attr in attributes)
		{
			options.MapType(attr.DataType, () => new OpenApiSchema
			{
				Type = attr.Type,
				Format = attr.Format,
				Pattern = attr.Pattern,
				Nullable = attr.Nullable,
			});
		}
	}
}
```
            
### XML
.NET supports XML Serialization out-of-the-box. All SVO's implement `IXmlSerialization`
with the same approach:
``` C#
XmlSchema IXmlSerializable.GetSchema() => null;

void IXmlSerializable.ReadXml(XmlReader reader)
{
    var s = reader.ReadElementString();
    var val = Parse(s, CultureInfo.InvariantCulture);
    m_Value = val.m_Value;
}

void IXmlSerializable.WriteXml(XmlWriter writer)
{
    writer.WriteString(ToString(SerializableFormat, CultureInfo.InvariantCulture));
}
```

## Qowaiv SVO options

### Hashing
To support hashing (`object.GetHashCode()`) the hash code should always return 
the same value, for the same object. As SVO's are equal by value, the hash
is calculated based on the underlying value.

Due to IXmlSerialization support, however, the underlying value is not
read-only, because this interface first create default instance and then
sets the value. Only if somebody intentionally misuses the `IXmlSerialization`
interface, can change a value during the lifetime of a SVO.

Therefor

``` CSharp
#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing
```

is fine.

### Sortable
SVO's support sorting. So, LINQ expressions like OrderBy() and OrderByDescending()
work out of the box, just like Array.Sort(), and List.Sort(). However, the
comparison operators (<, >, <=, >=) do only make sense for a subset of those,
and are not implemented on all.

Therefor

``` CSharp
#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
```

is fine for types that are sortable via IComparable (in most cases).

### Debugger display
During debugging sessions, by default, the IDE shows the result of ToString()
on a watch. Although Tostring() is overridden for all Qowaiv Single Value 
Objects, for debugging a special debugger display is provided too, using a 
debugger display attribute.

The debugger display attribute refers to (private) property with the name 
"DebuggerDisplay", which represents the Single Value Object as string. If 
supported, formatted, and in case of a Empty or Unknown value with a 
notification of that too. The outcome of the DebuggerDisplay is tested in the 
UnitTests.

Because the rendering of debugger display is handled based on the development 
environment, and methods as debugger display are not supported by VB.NET, the 
debugger display attribute refers to a property instead.

## Qowaiv Formatting
Formatting is an important part of the functionality in Qowaiv. All SVO's 
implement IFormattable, and have custom formatting. For details, see the 
different remarks at the ToString(string, IFormatProvider).

### Formatting arguments
The formatting arguments object, is a container object (struct) of the format 
and the format provider, the two arguments required for the System.Iformatable 
ToString() method.

### Formatting arguments collection
This collection of formatting arguments stores them based on a type to apply 
on. On top of that, it has a Format() method, that is an extended implementation 
of string.Format(). The difference between these two methods is, that - when no 
custom format is supplied at the format string - string.Format() the default 
formatting of the object is used, where FormattingArgumentsCollection.Format() 
uses the default specified at the formatting collection of a type (if available).

### IConvertable
The `IConvertable` interface has limited value for other types than the .NET
primitives. However, for some old constructs (like VB.NET's `CStr()`), just
having an basic implementation helps.

For that reason, all methods required by the interface are only explicitly
accessable.

### Threading
Because there are scenario's where you want to set typical values as a country 
or a currency for the context of the current thread (like the culture info) 
there is a possibility to add these to the Qowaiv.Threading.ThreadDomain.

These values can be configured (in the application settings) or can be created with
a creator function that can be registered. If not specified otherwise the current 
country will be created (if possible) based on the current culture.

## Qowaiv clock
The `Clock` class is an outsider within the Qowaiv library. It is a solution 
for a problem that is not related to Domain-Driven Design, but to the fact that
the behaviour of `System.DateTime.UtcNow` (and its equivalents) can not be controlled.
This can be problematic for writing proper tests that relay on its behaviour.

The default way to tackle this problem is by providing a lightweight service 
like this one:
``` CSharp
public interface IClock
{
    DateTime UtcNow();
}

public class Clock : IClock
{
	public DateTime UtcNow() => DateTime.UtcNow;
}
```

However, providing an IClock all the time when there is time related logic is
not that elegant at all. The Qowaiv `Clock` helps to overcome this. In code
you just call `Clock.UtcNow()` or one of its derived methods. In a test you
change the behaviour, in most cases just for the scope of your current threat:
``` CSharp
[Test]
public void TestSomething()
{
    using(Clock.SetTimeForCurrentThread(() => new DateTime(2017, 06, 11))
    {
        // test code.
    }
}
```
