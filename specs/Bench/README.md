# Qowaiv Benchmarks

## Amount
Amount JSON serialization is equivalent to decimal JSON serialization:

| Method  | Categories           | Mean     | Ratio |
|-------- |--------------------- |---------:|------:|
| Decimal | JSON Deserialization | 51.81 us |  1.00 |
| Amount  | JSON Deserialization | 50.68 us |  0.98 |
|         |                      |          |       |
| Decimal | JSON Serialization   | 26.88 us |  1.00 |
| Amount  | JSON Serialization   | 27.49 us |  1.02 |

## IBAN
The IBAN parser has been rewritten multiple times. Unil v6.6.0 it was regex
based. Since then, it is a specialized parser, that uses (read-only) spans,
BBAN lookups, assumes normalized input first, and tries to skip minimializes
the mod 97 to the bear minimum.

Bother Iban.NET and the regex implementation are added for reference. Notice
that the regex implementation is already way faster than regex based version
that was used within Qowaiv in the past.

| Method   | Categories | Mean      | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|----------|------------|----------:|------:|--------:|-------:|----------:|------------:|
| Qowaiv   | Normalized |  55.64 us |  1.00 |       - |      - |    0.3 kB |        1.00 |
| Iban.NET | Normalized | 219.12 us |  3.94 | 12.4512 | 3.9063 |  159.0 kB |      473.31 |
| Regex    | Normalized | 240.07 us |  4.32 | 23.4375 | 4.6387 |  294.0 kB |      875.19 |
|          |            |           |       |         |        |           |             |
| Qowaiv   | Formatted  |  95.10 us |  1.00 |  5.7373 | 1.3428 |   72.6 kB |        1.00 |
| Iban.NET | Formatted  | 220.01 us |  2.32 | 12.4512 | 3.9063 |  159.0 kB |        2.19 |
| Regex    | Formatted  | 244.84 us |  2.58 | 24.1699 | 4.6387 |  305.1 kB |        4.20 |

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

### System.Text.Json Serialization
| Method     | Categories           | Mean     | Ratio |
|----------- |--------------------- |---------:|------:|
| Decimal    | JSON Deserialization | 42.65 us |  1.00 |
| Percentage | JSON Deserialization | 52.71 us |  1.24 |
|            |                      |          |       |
| Decimal    | JSON Serialization   | 25.65 us |  1.00 |
| Percentage | JSON Serialization   | 39.77 us |  1.55 |

## UUID
The Base64 (default) implementation of a UUID is comparable with GUID.

| Method        | Categories | Mean      | Ratio |
|-------------- |----------- |----------:|------:|
| System.Guid   | Parse()    | 15.773 ns |  1.00 |
| UUID.Base16   | Parse()    | 25.355 ns |  1.61 |
| UUID.Base64   | Parse()    | 17.381 ns |  1.10 |
| UUID.Base32   | Parse()    | 28.491 ns |  1.81 |
| GUID based ID | Parse()    | 33.180 ns |  2.11 |
| UUID based ID | Parse()    | 28.857 ns |  1.83 |
|               |            |           |       |
| System.Guid   | ToString() |  7.952 ns |  1.00 |
| UUID.Base16   | ToString() | 36.774 ns |  4.63 |
| UUID.Base64   | ToString() | 17.795 ns |  2.24 |
| UUID.Base32   | ToString() | 51.867 ns |  6.53 |
| GUID based ID | ToString() |  3.420 ns |  0.43 |
| UUID based ID | ToString() |  3.306 ns |  0.42 |
