![Qowaiv](https://github.com/Qowaiv/Qowaiv/blob/master/design/qowaiv-logo_linkedin_100x060.jpg)

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Code of Conduct](https://img.shields.io/badge/%E2%9D%A4-code%20of%20conduct-blue.svg?style=flat)](https://github.com/Qowaiv/Qowaiv/blob/master/CODE_OF_CONDUCT.md)
![Build Status](https://github.com/Qowaiv/Qowaiv/workflows/Build%20%26%20Test/badge.svg?branch=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Qowaiv_Qowaiv&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Qowaiv_Qowaiv)
[![Coverage Status](https://coveralls.io/repos/github/Qowaiv/Qowaiv/badge.svg?branch=master)](https://coveralls.io/github/Qowaiv/Qowaiv?branch=master)

| version                                                                          | downloads                                                        | package                                                                        |
|----------------------------------------------------------------------------------|------------------------------------------------------------------|--------------------------------------------------------------------------------|
|![v](https://img.shields.io/nuget/v/Qowaiv?color=18C)                             |![v](https://img.shields.io/nuget/dt/Qowaiv)                      |[Qowaiv](https://www.nuget.org/packages/Qowaiv/)                                |
|![v](https://img.shields.io/nuget/v/Qowaiv.Data.SqlClient?color=18C)              |![v](https://img.shields.io/nuget/dt/Qowaiv.Data.SqlClient)       |[Qowaiv.Data.SqlCient](https://www.nuget.org/packages/Qowaiv.Data.SqlClient/)   |
|![v](https://img.shields.io/nuget/v/Qowaiv.Qowaiv.Diagnostics.Contracts?color=118)|![v](https://img.shields.io/nuget/dt/Qowaiv.Diagnostics.Contracts)|[Qowaiv.TestTools](https://www.nuget.org/packages/Qowaiv.Diagnostics.Contracts/)|
|![v](https://img.shields.io/nuget/v/Qowaiv.TestTools?color=118)                   |![v](https://img.shields.io/nuget/dt/Qowaiv.TestTools)            |[Qowaiv.TestTools](https://www.nuget.org/packages/Qowaiv.TestTools/)            |

# Qowaiv

## Domain-driven design bottom up
Qowaiv is a (Single) Value Object library. It aims to model reusable (Single)
Value Objects that can be used a wide variety of modeling scenarios, both
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
All Qowaiv SVO's have been created as `struct`, not as `class`. The reason for
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
* Serialization (JSON, XML, in-memory)
* Model binding
* Domain-specific logic
* Explicit and implicit casting

## Building your own
If you need a Single Value Object that is not provided by Qowaiv you can build
your own. How to do that can be read [here](README.Custom.SVO.md).

## Qowaiv types

### Date
Represents a date, so without hours (minutes, seconds, milliseconds), opposed to `DateTime`.

``` C#
var date = new Date(2017, 06, 11);
var next = date++; // 2017-06-12
var casted = (Date)new DateTime(2017, 06, 11, 06, 15);
```

As, since .NET 6.0, `System.DateOnly` is available, `Qowaiv.Date` can be casted to (and from)
this type, if (and only if), the .NET 6.0 version of the package is used.

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

``` C#
StreamSize fromMb = StreamSize.FromMegabytes(14.2);
StreamSize parsed = StreamSize.Parse("117.2Kb");
StreamSize humanizer = 142.23.KB(); // Bytes(), KiB(), MB(), MiB(), GB(), GiB()

// Short notation
new StreamSize(8900).ToString("s") => 8900b
new StreamSize(238900).ToString("s") => 238.9kb
new StreamSize(238900).ToString(" S") => 238.9 kB
new StreamSize(238900).ToString("0000.00 S") => 0238.90 kB

// Full notation
new StreamSize(8900).ToString("0.0 f") => 8900.0 byte
new StreamSize(238900).ToString("0 f") => 234 kilobyte
new StreamSize(1238900).ToString("0.00 F") => 1.24 Megabyte

// Custom
new StreamSize(8900).ToString("0.0 kb") => 8.9 kb
new StreamSize(238900).ToString("0.0 MB") => 0.2 MB
new StreamSize(1238900).ToString("#,##0.00 Kilobyte") => 1,239.00 Kilobyte
new StreamSize(1238900).ToString("#,##0") => 1,238,900
```

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
Month dec = (Month)12;

feb.ToString("f", new CultureInfo("nl-NL")); // februari
feb.ToString("s"); // Feb
feb.ToString("M"); // 02
feb.ToString("m"); // 2

// Querying on date (time) models.
new DateTime(2014, 02, 14).IsIn(feb); // true
```

### Month span
Is a subset of the date span, so without the days precision.

``` C#
// Creation
var ctor = new MonthSpan(years: 5, months: 6); // 69 months.
var months = MonthSpan.FromMonths(13);
var years = MonthSpan.FromYears(3); // 35 months.

// operations
var delta = MonthSpan.Subtract(new Date(2020, 04, 01), new Date(2020, 02, 28)); // 1 month.
var prev = new Date(2017, 06, 11) - MonthSpan.FromMonths(9); // 2016-09-11
var next = new DateTime(2010, 05, 02).Add(MonthSpan.FromMonths(2)); // 2010-07-02
```

### Percentage
Represents a percentage. It supports parsing from per mile and per ten thousand
too. The basic thought is that `Percentage.Parse("14%")` has the same result
as `double.Parse("14%")`, which is `0.14`.

``` C#
// Creation
var p = Percentage.Parse("3.14");  // Parse: 3.14%;
var p = Percentage.Parse("3.14%"); // Parse: 3.14%;
var p = Percentage.Parse("31.4‰"); // Parse: 3.14%;
var p = 3.14.Percent(); // Extension on double: 3.14%;
Percentage p = 0.0314m; // Implicit casting from decimal: 3.14%.

// Explicit casting
var p = (Percentage)0.500; // 50%

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

var perMille = 15.5.Percent().ToString("PM"); // 155‰
var perTenThousand = 0.34.Percent().ToString("PT"); // 34‱
```

### Postal code
Represents a postal code. It supports validation for all countries.

``` C#
var dutch = PostalCode.Parse("2624DP");
dutch.IsValid(Country.NL); // true
dutch.IsValid(Country.BE); // false

var argentina = PostalCode.Parse("Z1230ABC");
argentina.ToString("AR"); // Z 1230 ABC
```

### Sex
Represents a sex based on an ISO 5218 code.

``` C#
var sex = Sex.Female;
var parsed = Sex.Parse("Male");

sex.ToString("s"); // ♀
sex.ToString("c"); // f
sex.ToString(string.Empty, new CultureInfo("pt")); // Feminino
sex.ToString("h", new CultureInfo("en-GB")); // Mrs.
```

### Week date
Represents a week based date.

### Year
Represents a year in the range [1-9999].

``` C#
Year year = 2017.CE(); // Extention, Common Era
bool isLeap = year.IsLeapYear;

// behavior similar to double.NaN
(Year.Empty < 2000.CE()).Should().BeFalse();
(Year.Empty > 2000.CE()).Should().BeFalse();
(Year.Unknown < 2000.CE()).Should().BeFalse();
(Year.Unknown > 2000.CE()).Should().BeFalse();

// Querying on date (time) models.
new DateTime(2017, 02, 14).IsIn(year); // true
```

### Yes-no
A Yes-no is a (bi-)polar that obviously has the values "yes" and "no". It also
has an "empty"(unset) and "unknown" value. It maps easily with a `boolean`, but
Supports all kind of formatting (and both empty and unknown) that can not be
achieved when modeling a property as `bool` instead of an `YesNo`.

``` C#
// Creation
var yn = YesNo.Parse("ja", new CultureInfo("nl-NL"));
var answer = YesNo.Yes;

if (answer.IsEmptyOrUnknown())
{
   // Do logic.
}

if (answer) // Equal to answer.IsYes()
{
    // Do logic.
}
```

## Qowaiv chemistry types

### CAS Registry Number
A CAS Registry Number, is a unique numerical identifier assigned by the
Chemical Abstracts Service (CAS), US to every chemical substance described
in the open scientific literature. It includes all substances described
from 1957 through the present, plus some substances from as far back as the
early 1800's.

``` C#
// Creation
var fromString = CasRegistryNumber.Parse("7732-18-5");
var fromInt32 = 7732_18_5.CasNr();
var fromInt64 = 7732_18_5L.CasNr();
```

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
iban.ToString("F"); // NL20 INGB 0001 2345 67
iban.ToString("H"); // NL20 INGB 0001 2345 67 (with non-breaking spaces).
```

An overview with all supported countries and patterns can be found [here](IBAN.md).

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

### CultureInfoScope
A CultureInfoScope is a class that allows to specify the current (UI) for the
duration/lifetime of specified scope.

``` C#
using(new CultureInfoScope("es-ES"))
{
    Console.WriteLine(234.12.ToString()); // 234,12
}
// or with an extension
using(new CultureInfo("en-ES").Scoped())
{
    // ...
}
```

## Qowaiv mathimatical types

### Fraction
A fraction (from Latin _fractus_, "broken") represents a part of a whole or, more
generally, any number of equal parts. It can be written as a/b (a divided by b).

It is worth noticing that the `default` value of a `Fraction` is ``Fraction.Zero`
and that `NaN` and `Infinity` are not supported.

#### Creation
Fractions can be created in multiple ways:
``` C#
Fraction fluent = 4.DividedBy(13); // Preferred way

Fraction ctor = new(4, 13);

Fraction parsed = Fraction.Parse("4/13");
Fraction parsed = Fraction.Parse("4½"); // single fraction character
Fraction parsed = Fraction.Parse("4²³⁄₄₇"); // Unicode super- and subscript

Fraction fromFloating = Fraction.Create(0.3456786754m);

Fraction casted = (Fraction)34;
Fraction casted = (Fraction)0.3333;
```

#### Operations
``` C#
var add = 4.DividedBy(3) +  7.DividedBy(4); // 3 1/12
var subtract = 4.DividedBy(3) - 2.DividedBy(3); // 2/3
var multiply = 3.DividedBy(5) * 2.DividedBy(3); // 6/15
var divide = = 3.DividedBy(5) / 2.DividedBy(3); // 9/10
```

#### Formatting
There are multiple types of formatting supported. Without
a fraction bar, the fraction is formatted as a decimal.
``` C#
var dec = 17.DividedBy(5).ToString("0.##"); // 3,40
```

If the whole should be formatted as such the by adding the preferred formatting
between brackets.
``` C#
var withWhole = 17.DividedBy(5).ToString("[0]0/0"); // 3 2/5
``` 

To specify fraction bar of choice, just define that one in the format:
##### Fraction bars
name           |   c    | code 
---------------|:------:|------
slash          |   /    | 005C
colon          |   :    | 003A
division sign  |   ÷    | 00F7
fraction slash |   ⁄    | 2044
division slash |   ∕    | 2215
short slash    | &0337; | 0337
long slash     | &0338; | 0338

``` C#
var customBar = 3.DividedBy(4).ToString("0/÷0"); // 3÷4
```

Unicode supports super- and subscript, and so does `Fraction`:
```C#
var super = -17.DividedBy(5).ToString("[0]super/sub"); // -3²⁄₅
var super = 5.DividedBy(3).ToString("super/sub"); // ⁵⁄₃
```

The default format (so if you do not specify anything) is:
```C#
var basic = 3.DividedBy(4).ToString("0/0"); // 3/4
```

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

## Qowaiv identifiers

### Strongly-typed identifiers
Primitive Obsession is a common issue when dealing with identifiers. It is quite
common to provide two or even more identifiers (of different identities) to a
method, which can lead to nasty bugs.

To overcome this, strongly-typed identifiers can save the day: a specific type
per identifier per identity. Qowaiv's approach is to use an `Id<T>` where T is
a class dealing with the logic/behavior of the underlying values. This gives a
lot of flexibility, and requires hardly any code per identifier, as in 99% of
the cases, you can fully rely on a base implementation (Guid, long, int, string).

``` C#
// Definition of the identifiers.
public sealed class ForDocument : Int64IdLogic { }
public sealed class ForPerson : GuidLogic { }
public sealed class ForUser : GuidLogic { }

// Creation
var documentId = (Id<ForDocument>)123457L; // cast
var personId = Id<ForPerson>.Parse("0bb59085-9184-4df9-b9d4-08e1ba65cef8"); // parse
var userId = Id<ForUser>.Create(new Guid("0bb59085-9184-4df9-b9d4-08e1ba65cef8")); // create.
var bytesId = Id<forDocument>.FromBytes(new byte[]{ 17, 0, 0, 0, 0, 0, 0, 0 }); // create from bytes.
var nextId = Id<ForPerson>.Next(); // New ID, a random GUID in this case

var same = personId.Equals(userId); // false, same GUID, different types.

// Export
var docId = (long)documentId; // cast
var perId = personId.ToString(); // "0bb59085-9184-4df9-b9d4-08e1ba65cef8"
var bytId = bytesId.ToByteArray();

// Custom logic
public sealed class ForCustomer : StringIdLogic
{
    protected override bool IsValid(string str, out string normalized)
    {
        normalized = default;
        if (Regex.IsMatch("$C[1-9][0-9]{4,6}^"))
        {
            normalized = str;
            return true;
        }
        return false;
    }

    public override object Next() => 'C' + Rnd.Next(10_000, 9_999_999).ToString();
}

```

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

## Qowaiv security types

### Secret
Represents a (text based) secret. It tries to avoid exposing potentially
sensitive strings to log files or (external) devices. It does not defend
against sources that have (direct) access to the system memory. Consider
if this is secure enough for the problem at hand, before using this type.

A secret can be created by parsing, deserializing JSON, or using its type
converter to convert from string. serializing to JSON, or converting it
to another type are not supported, and `ToString()` returns `*****`. The
only way to access its value is by calling the `Value()` method.

``` C#
var secret = Secret.Parse("Ken sent me!");
var encrypted = secret.ComputeHash(sha516);
```

## Cryptographic Seed
Represents a byte array based cryptographic seed. It tries to avoid exposing
potentially sensitive data to log files or (external) devices. It does not
defend against sources that have (direct) access to the system memory. Consider
if this is secure enough for the problem at hand, before using this type.

A secret can be created by parsing, deserializing JSON, or using its type
converter to convert from string. serializing to JSON, or converting it to
another type are not supported, and `ToString()` returns `*****`. The only way
to access its value is by calling the `Value()` or `ToByteArray()` method.

``` C#
var seed = CryptographicSeed.Parse("S2VuIHNlbnQgbWUhIQ=="); // Base64 string
var seed = sha516.ComputeCryptographicSeed(new byte[]{ 0xD4, 0x1D, 0x8C, 0xD9 });
```

## Qowaiv statistical types

### Elo
Represents an Elo (rating), a method for calculating the relative skill levels of
players in competitor-versus-competitor games.

``` C#
var p0 = 1600.Elo();
var p1 = 1500.Elo();
var z = Elo.GetZScore(p0, p1); // 0.64 
```

## Qowaiv sustainability types

### (EU) Energy label
The energy efficiency of the appliance is rated in terms of a set of energy
efficiency classes from A++++ to G on the label, A being the most energy
efficient, G the least efficient.

``` C#
var a_plusplus = EnergyLabel.A(2);
var b = EnergyLabel.B;
var parsed = EnergyLabel.Parse("A++")
```

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
    public static Svo FromJson(string? json);

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
There are two _out-of-the-box_ implementations that support this convention
based contract.
* [Qowaiv.Json.Newtonsoft](https://www.nuget.org/packages/Qowaiv.Json.Newtonsoft/)  
* [Qowaiv.Text.Json.Serialization](https://www.nuget.org/packages/Qowaiv.Text.Json.Serialization/)  

For the .NET 5.0, and higher versions of the package, when using `System.Text.Json`,
no custom serialization registration is required for Qowaiv SVO's: all have been
decorated with the `[JsonConverter]` attribute.

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
    "description": "Full-date notation as defined by RFC 3339, section 5.6.",
    "example": "2017-06-10",
    "type": "string",
    "format": "date",
    "nullabe": false
  },
  "DateSpan": {
    "description": "Date span, specified in years, months and days.",
    "example": "1Y+10M+16D",
    "type": "string",
    "format": "date-span",
    "pattern": "[+-]?[0-9]+Y[+-][0-9]+M[+-][0-9]+D",
    "nullabe": false
  },
  "EmailAddress": {
    "description": "Email notation as defined by RFC 5322.",
    "example": "svo@qowaiv.org",
    "type": "string",
    "format": "email",
    "nullabe": true
  },
  "EmailAddressCollection": {
    "description": "Comma separated list of email addresses defined by RFC 5322.",
    "example": "info@qowaiv.org,test@test.com",
    "type": "string",
    "format": "email-collection",
    "nullabe": true
  },
  "HouseNumber": {
    "description": "House number notation.",
    "example": "13",
    "type": "string",
    "format": "house-number",
    "nullabe": true
  },
  "LocalDateTime": {
    "description": "Date-time notation as defined by RFC 3339, without time zone information.",
    "example": "2017-06-10 15:00",
    "type": "string",
    "format": "local-date-time",
    "nullabe": false
  },
  "Month": {
    "description": "Month(-only) notation.",
    "example": "Jun",
    "type": "string",
    "format": "month",
    "nullabe": true,
    "enum": ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "?"]
  },
  "MonthSpan": {
    "description": "Month span, specified in years and months.",
    "example": "1Y+10M",
    "type": "string",
    "format": "month-span",
    "pattern": "[+-]?[0-9]+Y[+-][0-9]+M",
    "nullabe": false
  },
  "Percentage": {
    "description": "Ratio expressed as a fraction of 100 denoted using the percent sign '%'.",
    "example": "13.76%",
    "type": "string",
    "format": "percentage",
    "pattern": "-?[0-9]+(\\.[0-9]+)?%",
    "nullabe": false
  },
  "PostalCode": {
    "description": "Postal code notation.",
    "example": "2624DP",
    "type": "string",
    "format": "postal-code",
    "nullabe": true
  },
  "Sex": {
    "description": "Sex as specified by ISO/IEC 5218.",
    "example": "female",
    "type": "string",
    "format": "sex",
    "nullabe": true,
    "enum": ["NotKnown", "Male", "Female", "NotApplicable"]
  },
  "Uuid": {
    "description": "Universally unique identifier, Base64 encoded.",
    "example": "lmZO_haEOTCwGsCcbIZFFg",
    "type": "string",
    "format": "uuid-base64",
    "nullabe": true
  },
  "WeekDate": {
    "description": "Full-date notation as defined by ISO 8601.",
    "example": "1997-W14-6",
    "type": "string",
    "format": "date-weekbased",
    "nullabe": false
  },
  "Year": {
    "description": "Year(-only) notation.",
    "example": 1983,
    "type": "integer",
    "format": "year",
    "nullabe": true
  },
  "YesNo": {
    "description": "Yes-No notation.",
    "example": "yes",
    "type": "string",
    "format": "yes-no",
    "nullabe": true,
    "enum": ["yes", "no", "?"]
  },
  "Chemistry.CasRegistryNumber": {
    "description": "CAS Registry Number",
    "example": "7732-18-5",
    "type": "string",
    "format": "cas-nr",
    "pattern": "[1-9][0-9]+\\-[0-9]{2}\\-[0-9]",
    "nullabe": true
  },
  "Financial.Amount": {
    "description": "Decimal representation of a currency amount.",
    "example": 15.95,
    "type": "number",
    "format": "amount",
    "nullabe": false
  },
  "Financial.BusinessIdentifierCode": {
    "description": "Business Identifier Code, as defined by ISO 9362.",
    "example": "DEUTDEFF",
    "type": "string",
    "format": "bic",
    "nullabe": true
  },
  "Financial.Currency": {
    "description": "Currency notation as defined by ISO 4217.",
    "example": "EUR",
    "type": "string",
    "format": "currency",
    "nullabe": true
  },
  "Financial.InternationalBankAccountNumber": {
    "description": "International Bank Account Number notation as defined by ISO 13616:2007.",
    "example": "BE71096123456769.",
    "type": "string",
    "format": "iban",
    "nullabe": true
  },
  "Financial.Money": {
    "description": "Combined currency and amount notation as defined by ISO 4217.",
    "example": "EUR12.47",
    "type": "string",
    "format": "money",
    "pattern": "[A-Z]{3} -?[0-9]+(\\.[0-9]+)?",
    "nullabe": false
  },
  "Globalization.Country": {
    "description": "Country notation as defined by ISO 3166-1 alpha-2.",
    "example": "NL",
    "type": "string",
    "format": "country",
    "nullabe": true
  },
  "Identifiers.GuidBehavior": {
    "description": "GUID based identifier",
    "example": "8a1a8c42-d2ff-e254-e26e-b6abcbf19420",
    "type": "string",
    "format": "guid",
    "nullabe": true
  },
  "Identifiers.Int32IdBehavior": {
    "description": "Int32 based identifier",
    "example": 17,
    "type": "integer",
    "format": "identifier",
    "nullabe": true
  },
  "Identifiers.Int64IdBehavior": {
    "description": "Int64 based identifier",
    "example": 17,
    "type": "integer",
    "format": "identifier",
    "nullabe": true
  },
  "Identifiers.StringIdBehavior": {
    "description": "String based identifier",
    "example": "Order-UK-2022-215",
    "type": "string",
    "format": "identifier",
    "nullabe": true
  },
  "Identifiers.UuidBehavior": {
    "description": "UUID based identifier",
    "example": "lmZO_haEOTCwGsCcbIZFFg",
    "type": "string",
    "format": "uuid-base64",
    "nullabe": true
  },
  "IO.StreamSize": {
    "description": "Stream size notation (in byte).",
    "example": 1024,
    "type": "integer",
    "format": "stream-size",
    "nullabe": false
  },
  "Mathematics.Fraction": {
    "description": "Faction",
    "example": "13/42",
    "type": "string",
    "format": "faction",
    "pattern": "-?[0-9]+(/[0-9]+)?",
    "nullabe": false
  },
  "Statistics.Elo": {
    "description": "Elo rating system notation.",
    "example": 1600.0,
    "type": "number",
    "format": "elo",
    "nullabe": false
  },
  "Sustainability.EnergyLabel": {
    "description": "EU energy label",
    "example": "A++",
    "type": "string",
    "format": "energy-label",
    "pattern": "[A-H]|A\\+{1,4}",
    "nullabe": true
  },
  "Web.InternetMediaType": {
    "description": "Media type notation as defined by RFC 6838.",
    "example": "text/html",
    "type": "string",
    "format": "internet-media-type",
    "nullabe": true
  }
}
```
  
#### Open API using Swagger 
When using [Swagger](https://swagger.io/resources/open-api/) to communicate
an Open API definition, this could be done like below:
``` C#
/// <summary>Extensions on <see cref="SwaggerGenOptions"/>.</summary>
public static class SwaggerGenOptionsSvoExtensions
{
    /// <summary>Maps Qowaiv SVO's.</summary>
    public static SwaggerGenOptions MapSingleValueObjects(this SwaggerGenOptions options)
    {
        var infos = OpenApiDataTypes.FromAssemblies(typeof(Date).Assembly);
        foreach (var info in infos)
        {
            options.MapType(info.DataType, () => new OpenApiSchema
            {
                Type = info.Type,
                Example = info.Example(),
                Format = info.Format,
                Pattern = info.Pattern,
                Nullable = info.Nullable,
            });
        }
        return options;
    }

    private static IOpenApiAny Example(this OpenApiDataType info)
        => info.Example switch
        {
            int integer => new OpenApiInteger(integer),
            double floating => new OpenApiDouble(floating),
            _ => new OpenApiString(attr.Example.ToString()),
        };
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
    // This is a work-around to keep the structs read-only.
    System.Runtime.CompilerServices.Unsafe.AsRef(this) = val;
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

For security messures, however, this is only true within the same app domain.
By having different hashes for the same value for different app domains, there
is good defense against hash flooding.

for test purposes, it is possible to generate a hashcode without using the randomizer:
``` C#
using(Hash.WithoutRandomizer())
{
    var hash = Hash.Code("QOWAIV string");
    hash.Should().Be(1211348473);
}
```

``Hash.Code()` also supports a fluent syntax, to get hashcodes for complex objects:
``` C#
public int GetHashCode()
    => Hash.Code(Prop)
    .And(Other)
    .And(Collection); // takes all items into account
```

This works out of the box because `Hash` can be implicitly cast to an `int`.
Calling `Hash.GetHashCode()` is not allowed, just use the implicit cast.


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
different remarks at the `ToString(string, IFormatProvider)`.

All SVO's support the `ICustomFormatter` interface; if the `IFormatProvider`
returns an `ICustomFormatter` on the `GetFormat(Type? type)` call and the
custom formatter actually returns a non-null string that formatted result is
returned by `ToString(string, IFormatProvider)`.

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
change the behaviour, in most cases just for the scope of your current 
(execution) context:

``` CSharp
[Test]
public void TestSomething()
{
    using(Clock.SetTimeForCurrentContext(() => new DateTime(2017, 06, 11))
    {
        // test code.
    }
}
```

### TimeProvider
Since .NET 8.0, Microsoft provides a `TimeProvider`. To benefit from both the
`Qowaiv.Clock` mechanism, and this time provider, the `Clock.TimeProvider`,
a singleton which provides access to `Clock.UtcNow()` and `Clock.TimeZone` is
added.

# Qowaiv Diagnostics Contracts
This packages contains attributes to define (expected) behavior on code
[(..)](src/Qowaiv.Diagnostics.Contracts/README.md)
