# Qowaiv Benchmarks

## IBAN
The first IBAN parse relied on regular expressions to validate the format. With
the v6.6.0 rewrite, regexes where removed, amongst other improvements which
made the parsing 14 times faster. With v8, The number of mod97's executes has
been drasticially reduced, leading to another big improvement.

| Parse         | Mean       | Ratio |
|---------------|-----------:|------:|
| Qowaiv v8     |    85.2 ns |  1.00 |
| Qowaiv v6.6.0 |   149.8 ns |  1.76 |
| IBAN.NET      |   196.1 ns |  2.30 |
| Regex based   | 2,138.8 ns | 25.10 |

Formatted v.s. unformatted strings have hardly any effect on the durations.


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

## Email address
The specialized parser (not using any `Regex`) guarantees fast parsing.
| Method | Mean     |
|------- |---------:|
| Parse  | 74.35 ns |

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

| Method            | Mean     | Error    | StdDev    | Median   | Ratio | RatioSD |
|------------------ |---------:|---------:|----------:|---------:|------:|--------:|
| GUID_Parse        | 52.24 us | 5.929 us | 17.480 us | 40.97 us |  1.11 |    0.50 |
| UUID_Parse        | 39.84 us | 0.822 us |  2.292 us | 39.53 us |  0.84 |    0.25 |
| UUID_Parse_Base64 | 28.15 us | 0.563 us |  1.606 us | 27.93 us |  0.60 |    0.18 |
| UUID_Parse_Base32 | 40.01 us | 0.774 us |  1.942 us | 39.63 us |  0.85 |    0.25 |

Reduced the function calls, and string replacements.

| Uuid.ToString()      | Mean     | Ratio |
|--------------------- |---------:|------:|
| GUID (Guid.ToString) | 12.53 us |  1.00 |
| Base64               | 18.11 us |  1.45 |
| GUID                 | 50.66 us |  4.04 |
| Convert.ToBase64     | 59.49 us |  4.75 |

The version of a `UUID` is stored in the upper 4 bits of byte 7. By using the
`GuidLayout` instead of `ToByteArray()` to retrieve that those bits is a big
improvement.

| Version     | Mean       | Ratio |
|------------ |-----------:|------:|
| Layout      |   0.584 ns |  1.00 |
| From byte[] |   3.024 ns |  5.20 |
