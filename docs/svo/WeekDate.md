# Week date
Represents a week based date.

``` C#
var date = new WeekDate(2017, 23, 7);
var next = date++; // 2017-W24-1
var (year, week, day) = date; // deconstructs.
```
