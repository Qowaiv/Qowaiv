# Qowaiv Benchmarks

## IBAN
By removing `Regex`'s from IBAN, durations of parsing have been reduced
dramatically.

| Parse (unformatted) | Mean       | Ratio |
|---------------------|-----------:|------:|
| BBAN                |   149.8 ns |  1.00 |
| Regex               | 2,138.8 ns | 14.29 |
| Regex (with tweaks) | 1,425.3 ns |  9.54 |

Formatted v.s. umformatted strings have hardly any effect on the durations.

| Parse (formatted)   | Mean       | Ratio |
|-------------------- |-----------:|------:|
| BBAN                |   151.8 ns |  1.00 |
| Regex               | 2,150.6 ns | 14.19 |
| Regex (with tweaks) | 1,359.1 ns |  8.98 |


## UUID
By removing `Regex` for the UUID parsing, durations have been reduced.
| Parse              | Mean     | Ratio |
|------------------- |---------:|------:|
| GUID (Guid.Parse)  | 15.63 ns |  1.00 |
| GUID               | 18.77 ns |  1.20 |
| Base64             | 18.58 ns |  1.19 |
| Base32             | 27.82 ns |  1.78 |
| Regex + FromBase64 | 92.76 ns |  5.94 |

The version of a `UUID` is stored in the upper 4 bits of byte 7. By using the
`GuidLayout` of `ToByteArray()` to retrieve that those bits is a big improvement.

| Method      | Mean       | Ratio |
|------------ |-----------:|------:|
| Layout      |   0.584 ns |  1.00 |
| From byte[] |   3.024 ns |  5.20 |
