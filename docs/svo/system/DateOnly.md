# System.DateOnly
The [`DateOnly`](https://learn.microsoft.com/dotnet/api/system.dateonly) type
was introduced with .NET 6.0. For previous versions we still
recommend [`Qowaiv.Date`](docs/svo/Date.md). Interaction with Qowaiv SVO's is provided:

``` C#
var more = new DateOnly(2017, 06, 11) + 3.Months();
var less = new DateOnly(2017, 06, 11) - 3.Months();

var more = new DateOnly(2017, 06, 11) + DateSpan.FromDays(3);
var less = new DateOnly(2017, 06, 11) - DateSpan.FromDays(3);
```
