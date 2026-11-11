# Month span
Is a subset of the date span, so without the days precision.

``` C#
// Creation
var ctor = new MonthSpan(years: 5, months: 6); // 69 months.
var months = MonthSpan.FromMonths(13);
var years = MonthSpan.FromYears(3); // 36 months.
var (years, months) = ctor; // deconstructs.
var humanizer = 5.Years() + 6.Months();

// operations
var delta = MonthSpan.Subtract(new Date(2020, 04, 01), new Date(2020, 02, 28)); // 1 month.
var prev = new Date(2017, 06, 11) - MonthSpan.FromMonths(9); // 2016-09-11
var next = new DateTime(2010, 05, 02).Add(MonthSpan.FromMonths(2)); // 2010-07-02
```
