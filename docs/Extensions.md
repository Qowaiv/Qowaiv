# Extensions
The Qowaiv core library comes with multiple extensions. They are added to improve
the day to day usagage of the SVOs provided.

## Humanizer
Inspired by [Humanizer.NET](https://humanizr.net), humanizer extension methods
create SVO instances:

```csharp
305_500.50m.Amount();       // Amount
4.DividedBy(7);             // Fraction
12.14m.Percent();           // Percentage
123_456_789.Bytes();        // StreamSize

5.Years();                  // YearSpan
5.Years() + 3.Months();     // MonthSpan
3.Months() + 5.Days();      // DateSpan
2017.CE();                  // Year

12345.CasNr();              // CasRegistryNumber
1500.Elo();                 // Elo
```

## Date and time

### Adding
The `DateSpan`, `MonthSpan`, and `YearSpan` can be added to `System.DateTime`,
`System.DateTimeOffset`, and `System.DateOnly`:

```csharp
var date = new DateTime(2017, 06, 11);
date.Add(4.Days());
date.Add(3.Months());
date.Add(10.Years());
```

### Range checks
`System.DateTime`, `System.DateTimeOffset`, and `System.DateOnly`, can be
checked to be in a specific month or year:

```csharp
var date = new DateOnly(2017, 06, 11);
date.IsIn(Month.January);
date.IsIn(2025.CE());
```
### Local date time
A `System.DateTimeOffset` can be converted to a `Qowaiv.LocalDateTime`:

```csharp
var date = new DateTimeOffset(2017, 06, 11);
var local = date.ToLocal();
```

## Conversion

### ToDecimal
Casts a `double` to a `decimal`, optionally rounding with `DecimalRounding`.

```csharp
1.2345.ToDecimal();
1.2345.ToDecimal(2, DecimalRounding.ToEven);
```

### ZeroIfNull
Returns zero if a nullable `INumberBase<T>` is null.

```csharp
int? value = null;
value.ZeroIfNull(); // 0
```

## Decimal math
All methods of `Qowaiv.Mathematics.DecimalMath` are available as extension methods.

## Reflection
All methods of `Qowaiv.Reflection.QowaivType` on `System.Type` are available as extension methods.

## Formatting

### Uppercase
Converts a string to uppercase using `System.Globalization.TextInfo` provided
by the specified format `System.IFormatProvider`.

```csharp
"hello".ToUpper(new CultureInfo("nl"));
```

### Title case
Applies the title case using `System.Globalization.TextInfo` provided
by the specified format `System.IFormatProvider`.

```csharp
"hello world".ToTitleCase(new CultureInfo("nl"));
```

### GetFormat
Gets a typed formatting service instance from an `IFormatProvider`.

```csharp
TextInfo info = formatProvider.GetFormat<TextInfo>();
```

## Stream Size
The `StreamSize` can be achieved from a `byte[]`, `System.IO.DirectoryInfo`,
`System.IO.FileInfo`, or `System.IO.Stream`.

```csharp
byteArray.GetStreamSize();
directoryInfo.GetStreamSize();
fileInfo.GetStreamSize();
stream.GetStreamSize();
```

### ToCSharpString
Gets a C# formatted string representation of a `Type`.

```csharp
typeof(List<string>).ToCSharpString();
typeof(List<string>).ToCSharpString(withNamespace: true);
```

## Security

### ComputeCryptographicSeed
Computes a `CryptographicSeed` from a byte array using a hash algorithm.

```csharp
using var sha = SHA256.Create();
var seed = sha.ComputeCryptographicSeed(bytes);
```

## Threading

### GetValue / SetValue / RemoveValue
Gets, sets, or removes values in the current thread's logical context.

```csharp
Thread.CurrentThread.SetValue("value");
string? v = Thread.CurrentThread.GetValue<string>();
Thread.CurrentThread.RemoveValue(typeof(string));
```

## Culture

### Scoped
Creates a `CultureInfoScope` based on a `CultureInfo`.

```csharp
using (culture.Scoped())
{
    // ...
}
```

## Collections (LINQ-like)
Extensions on `IEnumerable<T>` for Qowaiv types are available via `Qowaiv.Linq`.

```csharp
amounts.Average();
amounts.Average(x => x.Total);
amounts.Sum();
amounts.Sum(x => x.Total);

moneys.Average();
moneys.Sum();

percentages.Average();
percentages.Sum();

streamSizes.Average();
streamSizes.Sum();

elos.Average();
```
