# Year
Represents a year in the range [1-9999].

``` C#
Year year = 2017.CE(); // Extension, Common Era
bool isLeap = year.IsLeapYear;

// behavior similar to double.NaN
(Year.Empty < 2000.CE()).Should().BeFalse();
(Year.Empty > 2000.CE()).Should().BeFalse();
(Year.Unknown < 2000.CE()).Should().BeFalse();
(Year.Unknown > 2000.CE()).Should().BeFalse();

// Querying on date (time) models.
new DateTime(2017, 02, 14).IsIn(year); // true
```
