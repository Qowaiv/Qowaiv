![Qowaiv](https://github.com/Qowaiv/Qowaiv/blob/master/design/qowaiv-logo_linkedin_100x060.jpg)

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Code of Conduct](https://img.shields.io/badge/%E2%9D%A4-code%20of%20conduct-blue.svg?style=flat)](https://github.com/Qowaiv/Qowaiv/blob/master/CODE_OF_CONDUCT.md)

| version                                                                        | package                           |
|--------------------------------------------------------------------------------|-----------------------------------|
|![v](https://img.shields.io/badge/version-4.0.2-blue.svg?cacheSeconds=3600)     | Qowaiv                            |
|![v](https://img.shields.io/badge/version-4.0.0-blue.svg?cacheSeconds=3600)     | Qowaiv.Data.SqlCient              |
|![v](https://img.shields.io/badge/version-0.0.1-green.svg?cacheSeconds=3600)    | Qowaiv.Validation.Abstractions    |
|![v](https://img.shields.io/badge/version-0.0.1-darkgreen.svg?cacheSeconds=3600)| Qowaiv.Validation.DataAnnotations |
|![v](https://img.shields.io/badge/version-0.0.1-darkgreen.svg?cacheSeconds=3600)| Qowaiv.Validation.Fluent          |
|![v](https://img.shields.io/badge/version-1.0.1-darkred.svg?cacheSeconds=3600)  | Qowaiv.TestTools                  |

# Qowaiv

## Domain-driven design bottom up

Qowaiv is a (Single) Value Object library. It aims to model reusable (Single)
Value Objects that can be used a wide variety of modeling scenarios, both
inside and outside a Domain-driven context.

Supported scenarios include parsing, formatting, validation, (de)serialization,
and domain specific logic.

# Single Value Object
A Value Object that can be represented by a single scalar.

## Technical requirements
Because we use .NET standard to support both .NET 4.5 (and higher) as .NET Standard (2.0)
the Visual Studio solution file requires VS2017.3 or higher. Visual Studio can be downloaded
here: [visualstudio.com/downloads](https://www.visualstudio.com/downloads/).

## Qowaiv types

### Date

Represents a date, so without hours (minutes, seconds, milliseconds).

### Elo
Represents an Elo (rating), a method for calculating the relative skill levels of
players in competitor-versus-competitor games.

### Email address
Represents a (single) email address. Support:
* Display names (are stripped)
* Comments (are removed)
* IP-based domains (normalized and surrounded by breakets)

Furthermore, the email address is normalized as a lowercase string, making it
case-insensitve.

``` C#
var email = EmailAddress.Parse("Test Account <TEST@qowaiv.org>");
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

### Percentage
Represents a percentage/per mile/per ten thousand.

### Postal code
Represents a postal code. It supports validation for all countries.

### UUID aka GUID
The UUID (Universally unique identifier) aka GUID (Globally unique identifier) is an
extension on the System.Guid. It is by default represented by a 22 length string, 
instead of a 32 length string.

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

### Bank Identifier Code (BIC)
Represents a BIC as specified in ISO 13616.

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

### Guard
Guard parameters, for centralizing and simplifying the argument checking.


## Qowaiv SVO options

### Hashing
To support hashing (object.GetHashCode()) the hash code should always return 
the same value, for the same object. As SVO's are equal by value, the hash
is calculated based on the underlying value.

Due to IXmlSerialization support, however, the underlying value is not
read-only, because this interface first create default instance and then
sets the value. Only if somebody intentionally misuses the IXmlSerialization
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

### Threading
Because there are scenario's where you want to set typical values as a country 
or a currency for the context of the current thread (like the culture info) 
there is a possibility to add these to the Qowaiv.Threading.ThreadDomain.

These values can be configured (in the application settings) or can be created with
a creator function that can be registered. If not specified otherwise the current 
country will be created (if possible) based on the current culture.

## MVC ModelBinding
When using Qowaiv with MVC (not ASP.NET core) you need a specific model binder.
This binder is needed because the default model binder don't call a type
converter in case the input is string.Empty.
The example can be found [here](src/Qowaiv.Web/Mvc/typeConverterModelBinder.cs).

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

## Validation
Qowaiv supports validation of different kind. Details can be found here:
* [Qowaiv validation abstractions](src/Qowaiv.Validation.Abstractions/README.md)
* [Qowaiv validation data annotations](src/Qowaiv.Validation.DataAnnotations/README.md)
* [Qowaiv validation FluentValidation.NET](src/Qowaiv.Validation.Fluent/README.md)
