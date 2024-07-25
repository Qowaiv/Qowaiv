# Qowaiv Benchmarks

## IBAN
By removing `Regex`'s from IBAN, durations of parsing have been reduced
dramatically.

| Parse (unformatted) | Mean       | Ratio |
|---------------------|-----------:|------:|
| BBAN                |   149.8 ns |  1.00 |
| Regex               | 2,138.8 ns | 14.29 |
| Regex (with tweaks) | 1,425.3 ns |  9.54 |

Formatted v.s. unformatted strings have hardly any effect on the durations.

| Parse (formatted)   | Mean       | Ratio |
|-------------------- |-----------:|------:|
| BBAN                |   151.8 ns |  1.00 |
| Regex               | 2,150.6 ns | 14.19 |
| Regex (with tweaks) | 1,359.1 ns |  8.98 |

## Decimal round
Custom rounding is slower than .NET's default implementation. A big part can
be explained by the fact that MS can compile two separate versions for Big
and Little Endian architectures. As a result, they can do some bit operation
tricks we can not rely on.

| Method       | Mean      | Ratio |
|------------- |----------:|------:|
| Math_Round   |  5.787 ns |  1.00 |
| Qowaiv v6    | 29.425 ns |  5.08 |
| Qowaiv v7    | 26.247 ns |  4.56 |

## Percentage
Apply a divide by 100, the default operation to convert a decimal to a
percentage. Note that `DecimalMath` and the division trim, but the multiplication
does not.

| Method            | 3.14     | 23.4326  | 100      |
|------------------ |---------:|---------:|---------:|
| DecimalMath.Scale | 11.10 ns | 13.96 ns | 16.05 ns |
| value / 100m      | 27.63 ns | 31.83 ns | 14.22 ns |
| value * 0.01m     | 10.31 ns | 12.26 ns | 13.87 ns |

## UUID
By removing `Regex` for the UUID parsing, durations have been reduced.
| Parse              | Mean     | Ratio |
|------------------- |---------:|------:|
| GUID (Guid.Parse)  | 15.63 ns |  1.00 |
| Base64             | 18.58 ns |  1.19 |
| GUID               | 18.77 ns |  1.20 |
| Base32             | 27.82 ns |  1.78 |
| Regex + FromBase64 | 92.76 ns |  5.94 |

Reduced the function calls, and string replacements.

| ToString             | Mean     | Ratio |
|--------------------- |---------:|------:|
| GUID (Guid.ToString) | 11.10 ns |  1.00 |
| Base64               | 17.47 ns |  1.57 |
| GUID                 | 43.03 ns |  3.85 |
| Convert.ToBase64     | 50.71 ns |  4.57 |

The version of a `UUID` is stored in the upper 4 bits of byte 7. By using the
`GuidLayout` instead of `ToByteArray()` to retrieve that those bits is a big
improvement.

| Version     | Mean       | Ratio |
|------------ |-----------:|------:|
| Layout      |   0.584 ns |  1.00 |
| From byte[] |   3.024 ns |  5.20 |
