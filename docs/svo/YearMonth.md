# Year-month
Represent a date with year and month precision.

``` C#
var date = new YearMonth(2017, 06);
var (year, month) = date; // deconstructs.

// Querying
date.IsIn(2017.CE()); // true
var addition = date.AddMonths(23); // new YearMonth(2019, 05)
var next = date++; // new YearMonth(2017, 07)
MonthSpan age = new YearMonth(2024, 08) - date; // 7 years and 2 months.
```
