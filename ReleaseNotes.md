### New in 3.2.4 (Released 2019/04/05)
* Introduction of Clock.NowWithOffset(TimezoneInfo) #45
* Fix LocalDateTime to always have DateTimeKind.Local and no loss of ticks
* Clock.UtcNow() is guaranteed to have DateTimeKind.Utc

### New in 3.2.3 (Released 2019/04/03)
* Introduced Clock (#44)

### New in 3.2.2 (Released 2018/10/22)
* JsonConverter only throws JSON exceptions #36

### New in 3.2.1 (Realesed 2018/10/12)
* TypeConverters support conversions from the underlying value type (#19)
* Added Month.Days(year) (#30)

### New in 3.2.0 (Released 2018/08/08)
* Added the country Kosovo (#22)
* Made SonarAnalyzer dependency a private asset (#26)
* Extended Qowaiv.ComponentModel.Result with factory methods (#24)
* Introduced Qowaiv.ComponentModel.DataAnnotations.AnyAttribute (#25)
* Fix in email address collection (#21)

# New in 3.1.3 (Released 2018/04/17)
* Fix in email address validation (#18)

### New in 3.1.2 (Released 2017/12/12)
* New Base-32 implementation

### New in 3.1.1 (Released 2017/11/22)
* Introduced Component Model validation

### New in 3.0.0.223 (Released 2016/11/30)
* Introduced namespaces Financial, Globalization, Security, Statistics, and moved types from root namespace to those.
* Introduced Money.

### New in 2.0.2.186 (Released 2016/04/15)
* Add Qowaiv.Text.Base32.
* Added explicit conversion from Gender to Byte.

### New in 2.0.1.179 (Released 2016/02/29)
* Internal refactoring based on SonarLint recommendations.
* Converted solution to Visual Studio 2015.
* Extended country and currency definitions.

### New in 2.0.0.164 (Released 2015/12/04)
* Added Qowaiv.Statistics.Elo.

### New in 2.0.0.159 (Released 2015/12/03)
* Fixed an issue with negative values for StreamSize.

### New in 2.0.0.150 (Released 2015/11/22)
* Fix NuGet dependencies and use .net 4.5.

### New in 2.0.0.149 (Released 2015/10/15)
* Made Qowaiv.Guard internal.

### New in 1.0.0.139 (Released 2015/10/12)
* Introduced a build based on Fake.
