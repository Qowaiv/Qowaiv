# Qowaiv Benchmarks

# IBAN
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
