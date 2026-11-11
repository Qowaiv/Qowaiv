# Date span
Represents a date span. As opposed to a `TimeSpan`, its duration is (a bit) resilient;
Adding one month to a date in January results in adding a different number of days, 
than adding one month to a date in March.

Date spans are particularly useful in scenarios for defining (and doing calculations on)
month-based periods, and ages (mostly in years and days).

``` C#
var span = new DateSpan(years: 3, months: 2, days: -4);
var age = DateSpan.Age(new Date(2017, 06, 11)); // 2Y+0M+121D on 2019-10-10
var duration = DateSpan.Subtract(new Date(2019, 06, 10), new Date(2017, 06, 11)); // 1Y+11M+30D
var date = new Date(2016, 06, 03).Add(age); // 2018-10-02
var humanizer = 10.Years() + 3.Months() - 5.Days();
var (years, months, days) = span; // deconstructs.
```
