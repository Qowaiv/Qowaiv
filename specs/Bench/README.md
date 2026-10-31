# Qowaiv Benchmarks

## Amount
Amount JSON serialization is equivalent to decimal JSON serialization:

| Method  | Categories           | Mean     | Ratio |
|-------- |--------------------- |---------:|------:|
| Decimal | JSON Deserialization | 51.81 ns |  1.00 |
| Amount  | JSON Deserialization | 50.68 ns |  0.98 |
|         |                      |          |       |
| Decimal | JSON Serialization   | 26.88 ns |  1.00 |
| Amount  | JSON Serialization   | 27.49 ns |  1.02 |

## ANSI
| Method    | Runtime   | Mean     | Ratio | Gen0   | Allocated |
|---------- |---------- |---------:|------:|-------:|----------:|
| String    | .NET 10.0 | 23.89 us |  2.10 | 4.8523 |   60888 B |
| TryFormat | .NET 10.0 | 11.39 us |  1.00 |      - |         - |
|           |           |          |       |        |           |
| String    | .NET 11.0 | 20.70 us |  1.93 | 4.8523 |   60888 B |
| TryFormat | .NET 11.0 | 10.72 us |  1.00 |      - |         - |

## IBAN
The IBAN parser has been rewritten multiple times. Until v6.6.0 it was regex
based. Since then, it is a specialized parser, that uses (read-only) spans,
BBAN lookups, assumes machine readable input first, and tries to reduce the
`mod 97` operations to the bare minimum.

Both the Iban.NET and the regex implementation are added for reference. Notice
that the regex implementation is already way faster than regex based version
that was used within Qowaiv in the past.

| Parsing  | Readable | Mean      | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|----------|----------|----------:|------:|--------:|-------:|----------:|------------:|
| Qowaiv   | machine  |  32.23 ns |  1.00 |       - |      - |    0.3 kB |        1.00 |
| Iban.NET | machine  | 220.85 ns |  6.85 | 12.4512 | 3.9063 |  159.0 kB |      473.31 |
| Regex    | machine  | 239.50 ns |  7.43 | 23.4375 | 4.6387 |  294.0 kB |      875.19 |
|          |          |           |       |         |        |           |             |
| Qowaiv   | human    |  81.42 ns |  1.00 |  5.7373 | 1.0986 |   72.6 kB |        1.00 |
| Iban.NET | human    | 216.51 ns |  2.66 | 12.4512 | 3.9063 |  159.0 kB |        2.19 |
| Regex    | human    | 238.68 ns |  2.94 | 24.1699 | 4.6387 |  305.1 kB |        4.20 |

Writting an array of 1000 items. The baseline is IBANs as strings.

| JSON write | Mean     | Ratio | Allocated |
|----------- |---------:|------:|----------:|
| Baseline   | 255.8 ns |  1.00 |  73.01 kB |
| Qowaiv     | 115.5 ns |  0.45 |  73.57 kB |

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
percentage. Note that `DecimalMath` and the division trim (the latter only
when the result allows it), but the multiplication does not.

The benchmark converts 1,000 random (rounded) decimals per invocation.

| Method  | Categories |  Per op.  | Ratio |
|-------- |----------- |----------:|------:|
| Trim    | 32 bit     | 13.379 ns |  2.40 |
| No trim | 32 bit     |  5.579 ns |  1.00 |
|         |            |           |       |
| Trim    | 96 bit     | 19.855 ns |  2.57 |
| No trim | 96 bit     |  7.735 ns |  1.00 |

`DecimalMath.Scale` is the fastest way to divide by 100 *and* remove trailing
zeros. The plain multiplication is faster, but it does not normalize the scale.


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

## System.Text.Json (De)Serialization
Reading or writen an array of 1000 items, mean is a single operation.

### Date
| Read       | Runtime   | Mean      | Ratio | Allocated |
|----------- |---------- |----------:|------:|----------:|
| DateOnly   | .NET 10.0 |  34.42 ns |  1.00 |  28.18 KB |
| Date       | .NET 10.0 |  37.62 ns |  1.10 |  40.07 KB |
|            |           |           |       |           |
| DateOnly   | .NET 11.0 |  29.01 ns |  1.00 |  28.18 KB | 
| Date       | .NET 11.0 |  32.98 ns |  1.15 |  40.07 KB |
		     
| Write      | Runtime   | Mean      | Ratio | Allocated |
|----------- |---------- |----------:|------:|----------:|
| DateOnly   | .NET 10.0 |  27.80 ns |  1.00 |      32 B |
| Date       | .NET 10.0 |  53.96 ns |  1.95 |      32 B |
|            |           |           |       |           |
| DateOnly   | .NET 11.0 |  22.07 ns |  1.00 |      32 B |
| Date       | .NET 11.0 |  43.82 ns |  1.99 |      32 B |

### IBAN
| Read       | Runtime   | Mean      | Ratio | Allocated |
|----------- |---------- |----------:|------:|----------:|
| String     | .NET 10.0 |  66.14 ns |  1.00 | 110.64 KB |
| Iban       | .NET 10.0 | 112.94 ns |  1.71 | 110.86 KB |
|            |           |           |       |           |
| String     | .NET 11.0 |  63.85 ns |  1.00 | 110.64 KB |
| Iban       | .NET 11.0 | 111.69 ns |  1.75 | 110.86 KB |
		     
| Write      | Runtime   | Mean      | Ratio | Allocated |
|----------- |---------- |----------:|------:|----------:|
| String     | .NET 10.0 |  32.60 ns |  1.00 |      32 B |
| Iban       | .NET 10.0 |  29.25 ns |  0.90 |      32 B |
|            |           |           |       |           |
| String     | .NET 11.0 |  27.76 ns |  1.00 |      32 B |
| Iban       | .NET 11.0 |  26.16 ns |  0.95 |      32 B |

### Percentage
| Read       | Runtime   | Mean      | Ratio | Allocated |
|----------- |---------- |----------:|------:|----------:|
| String     | .NET 10.0 |  50.25 ns |  1.00 |  76.91 KB |
| Decimal    | .NET 10.0 |  74.27 ns |  1.49 |  63.85 KB |
| Percentage | .NET 10.0 |  78.11 ns |  1.56 |  63.85 KB |
|            |           |           |       |           |
| String     | .NET 11.0 |  41.05 ns |  1.00 |  76.91 KB |
| Decimal    | .NET 11.0 |  53.52 ns |  1.31 |  63.85 KB |
| Percentage | .NET 11.0 |  70.32 ns |  1.72 |  63.85 KB |

| Write      | Runtime   | Mean      | Ratio | Allocated |
|----------- |---------- |----------:|------:|----------:|
| Percentage | .NET 10.0 |  51.17 ns |  1.00 |      32 B |
| YesNo      | .NET 10.0 |  65.89 ns |  1.29 |      32 B |
|            |           |           |       |           |
| Percentage | .NET 11.0 |  39.46 ns |  1.00 |      32 B |
| YesNo      | .NET 11.0 |  66.55 ns |  1.69 |      32 B |

### Yes/No
| Read       | Runtime   | Mean      | Ratio | Allocated |
|----------- |---------- |----------:|------:|----------:|
| String     | .NET 10.0 |  37.98 ns |  1.00 |  69.37 KB |
| Bool       | .NET 10.0 |  22.87 ns |  0.61 |  22.23 KB |
| YesNo      | .NET 10.0 |  32.90 ns |  0.88 |  19.27 KB |
|            |           |           |       |           |
| String     | .NET 11.0 |  34.64 ns |  1.00 |  69.37 KB |
| Bool       | .NET 11.0 |  22.51 ns |  0.66 |  22.23 KB |
| YesNo      | .NET 11.0 |  34.78 ns |  1.01 |  19.27 KB |
		     
| Write      | Runtime   | Mean      | Ratio | Allocated |
|----------- |---------- |----------:|------:|----------:|
| String     | .NET 10.0 |  26.00 ns |  1.00 |      32 B |
| Bool       | .NET 10.0 |  15.30 ns |  0.59 |      32 B |
| YesNo      | .NET 10.0 |  16.92 ns |  0.65 |      32 B |
|            |           |           |       |           |
| String     | .NET 11.0 |  20.41 ns |  1.00 |      32 B |
| Bool       | .NET 11.0 |  13.22 ns |  0.65 |      32 B |
| YesNo      | .NET 11.0 |  15.83 ns |  0.78 |      32 B |
