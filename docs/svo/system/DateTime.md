# System.DateTime
The [`DateTime`](https://learn.microsoft.com/dotnet/api/system.datetime) is the
type to use when representing date and time. Interaction with Qowaiv SVO's
is provided:

``` C#
var more = new DateTime(2017, 06, 11) + 3.Months();
var less = new DateTime(2017, 06, 11) - 3.Months();

var more = new DateTime(2017, 06, 11) + DateSpan.FromDays(3);
var less = new DateTime(2017, 06, 11) - DateSpan.FromDays(3);
```
