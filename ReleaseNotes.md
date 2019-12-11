# Release Notes

### Released 2019-12-11
#### Qowaiv 5.0.0
* JSON serialization is convention based. (#115) breaking
* Dropped `BankIdentifierCode`. (#116) breaking

#### Qowaiv.Data.SqlClient 5.0.0
* JSON serialization is convention based. (#115) breaking

#### Qowaiv.TestTools 3.0.0
* JSON serialization is convention based. (#115) breaking

### Released 2019-11-03
#### Qowaiv 4.1.0
* Introduction of `DateSpan` (#95)
* Money, Amount, Percentage and SteamSize have `Abs()` method (#96)
* Money, Amount, Percentage and SteamSize have `Round()` method (#96)
* Introduction of `DecimalRound.Round()` and `RoundToMultiple()` (#97)
* `Date.FromJson(DateTime)` ignores time part (#98) fix
* Null message `NotSupportedException_ConverterCanNotConvertFomString` (#100) fix

#### Qowaiv.Data.SqlClient 4.1.0
* Updated package depedency.
* Own internal Guard.

#### Qowaiv.TestTools 2.0.0
* Removed Qowaiv.TestTools.Validation.* (107) breaking
* DebuggerDisplayAssert supports nested classes. (#110) fix

### Released 2019-10-02
#### Qowaiv 4.0.6
* Support OpenAPI (#81)
* Fix on serialization Month (#88)
* Introduction of BusinessIdentifierCode (#89)
#### Qowaiv.TestTools 1.0.3
* Extend TestTools.SerializationTest with XmlSerialize() and XmlDeserialize() (#93)

### Released 2019-09-06
#### Qowaiv 4.0.5
* Added support of "mailto:"-prefix on email address (#78)
* Fix on '<' in display name on email address (#77)

### Released 2019-09-04
#### Qowaiv 4.0.4
* Added Percentage.Min() and Percentage.Max() (#73)
* Added Percentage.Round() (#74)

### Released 2019-08-25
#### Qowaiv 4.0.3
* Added Percent() extension method (#70)

### Released 2019-07-30
#### Qowaiv.TestTools 1.0.2
* Updated references

### Released 2019-07-28
#### Qowaiv 4.0.2
* Extended display name support for email address #69 

### Released 2019-07-26
#### Qowaiv 4.0.1 
* Added support for 12 new countries with IBAN patterns #65
* Fix Y-N parsing for non-English languages #66
#### Qowaiv.TestTools 1.0.1
* Fix in TestTools Assert (internal)

### New in 4.0.0 (Released 2019-07-09)
* .NET standard 2.0 only (#59)
* Drop of Qowaiv.Web, Qowaiv.Json.Newtonsoft packages
* Introduction of Qowaiv.TestTools (v1.0.0)

### New in 3.2.4 (Released 2019-04-05)
* Introduction of Clock.NowWithOffset(TimezoneInfo) #45
* Fix LocalDateTime to always have DateTimeKind.Local and no loss of ticks
* Clock.UtcNow() is guaranteed to have DateTimeKind.Utc

### New in 3.2.3 (Released 2019-04-03)
* Introduced Clock (#44)

### New in 3.2.2 (Released 2018-10-22)
* JsonConverter only throws JSON exceptions #36

### New in 3.2.1 (Realesed 2018-10-12)
* TypeConverters support conversions from the underlying value type (#19)
* Added Month.Days(year) (#30)

### New in 3.2.0 (Released 2018-08-08)
* Added the country Kosovo (#22)
* Made SonarAnalyzer dependency a private asset (#26)
* Extended Qowaiv.ComponentModel.Result with factory methods (#24)
* Introduced Qowaiv.ComponentModel.DataAnnotations.AnyAttribute (#25)
* Fix in email address collection (#21)

# New in 3.1.3 (Released 2018-04-17)
* Fix in email address validation (#18)

### New in 3.1.2 (Released 2017-12-12)
* New Base-32 implementation

### New in 3.1.1 (Released 2017-11-22)
* Introduced Component Model validation

### New in 3.0.0.223 (Released 2016-11-30)
* Introduced namespaces Financial, Globalization, Security, Statistics, and moved types from root namespace to those.
* Introduced Money.

### New in 2.0.2.186 (Released 2016-04-15)
* Add Qowaiv.Text.Base32.
* Added explicit conversion from Gender to Byte.

### New in 2.0.1.179 (Released 2016-02-29)
* Internal refactoring based on SonarLint recommendations.
* Converted solution to Visual Studio 2015.
* Extended country and currency definitions.

### New in 2.0.0.164 (Released 2015-12-04)
* Added Qowaiv.Statistics.Elo.

### New in 2.0.0.159 (Released 2015-12-03)
* Fixed an issue with negative values for StreamSize.

### New in 2.0.0.150 (Released 2015-11-22)
* Fix NuGet dependencies and use .net 4.5.

### New in 2.0.0.149 (Released 2015-10-15)
* Made Qowaiv.Guard internal.

### New in 1.0.0.139 (Released 2015-10-12)
* Introduced a build based on Fake.
