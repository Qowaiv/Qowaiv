# Extensions

The Qowaiv core library comes with multiple extensions.

## Date and time

### Add
Adds a `DateSpan`, `MonthSpan`, or `YearSpan` to a date/time value.

```csharp
DateTime.Now.Add(new DateSpan(1, 2, 3));
DateTime.Now.Add(1.Months());
DateTimeOffset.Now.Add(1.Years());
DateOnly.MaxValue.Add(1.Days()); // available on .NET 6+
```

### IsIn
Checks if a date is in a specific month or year.

```csharp
DateTime.Today.IsIn(Month.January);
DateOnly.Today.IsIn(2025.CE());
```

### ToLocal
Converts a `DateTimeOffset` to a `LocalDateTime`.

```csharp
DateTimeOffset.UtcNow.ToLocal();
```

## Formatting

### ToString
Formats an `IFormattable` using `FormattingArguments` or `FormattingArgumentsCollection`.

```csharp
someAmount.ToString(new FormattingArguments("0.00"));
```

## Strings

### ToUpper
Converts a string to uppercase using a specific format provider.

```csharp
"hello".ToUpper(CultureInfo.InvariantCulture);
```

### ToTitleCase
Converts a string to title case using a specific format provider.

```csharp
"hello world".ToTitleCase(CultureInfo.CurrentCulture);
```

### GetFormat
Gets a formatting service instance of the specified type from an `IFormatProvider`.

```csharp
formatProvider.GetFormat<NumberFormatInfo>();
```

## System types

### GetStreamSize
Gets the size as a `StreamSize` for a byte array, directory, file, or stream.

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

### UuidVersion
Gets the version of a `Guid`.

```csharp
guid.UuidVersion;
```

## Conversion

### ToDecimal
Casts a `double` to a `decimal`, optionally rounding with `DecimalRounding`.

```csharp
1.2345.ToDecimal();
1.2345.ToDecimal(2, DecimalRounding.ToEven);
```

### ZeroIfNull
Returns zero if a nullable `INumberBase<T>` is null (.NET 10+).

```csharp
int? value = null;
value.ZeroIfNull(); // 0
```

### Decimal math
All methods of `DecimalMath` are available as extension methods.

### Decimal rounding helpers
```csharp
DecimalRounding.ToEven.IsDirectRounding();
DecimalRounding.ToEven.IsNearestRounding();
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

Nullable sequences are also supported where applicable (returning `null` if empty).

## Humanizer
Inspired by [Humanizer.NET](https://humanizr.net), humanizer extension methods create SVO instances.

```csharp
305_500.50m.Amount();       // Amount
4.DividedBy(7);             // Fraction
3.Months() + 5.Days();      // DateSpan
5.Years() + 3.Months();     // MonthSpan
12.14m.Percent();           // Percentage
2017.CE();                  // Year
5.Years();                  // YearSpan
123_456_789.Bytes();        // StreamSize
12345.CasNr();              // CasRegistryNumber
1500.Elo();                 // Elo
```
