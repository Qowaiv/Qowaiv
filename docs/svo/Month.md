# Month
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
