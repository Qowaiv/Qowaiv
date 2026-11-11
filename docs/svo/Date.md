# Date
Represents a date, so without hours (minutes, seconds, milliseconds), opposed to `DateTime`.

``` C#
var date = new Date(2017, 06, 11);
var next = date++; // 2017-06-12
var casted = (Date)new DateTime(2017, 06, 11, 06, 15);
var (year, month, day) = date; // deconstructs.
```

As, since .NET 6.0, `System.DateOnly` is available, `Qowaiv.Date` can be cast to (and from)
this type, if (and only if), the .NET 8.0 version or higher of the package is used.
