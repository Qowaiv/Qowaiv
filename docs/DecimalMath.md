# DecimalMath
Math similar to `System.Math`, but with `decimal` precision. It provides the
constants `Epsilon`, `E` and `Pi`, and extension methods for `decimal` values,
including the rounding extensions described below.

## ‘Negative’ decimals
To round tenfold, hundredfold, etc. precision, a negative amount of decimals
can be specified:
``` C#
var tenfold = 1245.346m.Round(-1); // 1250m
var hundredfold = 1209m.Round(-2); // 1200m
```

## Multiple of
Rounding to a multiple of is supported:
``` C#
var multipleOf = 123.5m.RoundToMultiple(5m); //   125.0m
var multiple25 = 123.5m.RoundToMultiple(2.5m); // 122.5m
```

## Extra rounding methods
.NET supports rounding to even (Bankers rounding) and away from zero out-of-the-box.
Rounding methods like ceiling, floor, and truncate have limited support (0 decimals only),
and many others (to odd, half-way up, half-way down, e.o.) are missing.
By specifying the `DecimalRounding` 13 ways are supported.

``` c#
var toOdd = 23.0455m.Round(3, DecimalRounding.ToOdd); // 23.045m
var towardsZero = 23.5m.Round(DecimalRounding.TowardsZero); // 23m
var randomTie = 23.5m.Round(DecimalRounding.RandomTieBreaking); // 50% 23, 50% 24
```
