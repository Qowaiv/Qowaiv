# Qowaiv validation benchmarks

## Allowed Values attribute

| Method                | Mean       | Ratio |
|-----------------------|-----------:|------:|
| AllowedValues         | 321.043 us | 1.000 |
| Allowed&lt;T&gt;      |   2.775 us | 0.009 |


## In Range attribute

| IsValid()                | Mean       | Ratio |
|------------------------- |-----------:|------:|
| `[ComponentModel.Range]` |  40.587 us |  1.00 |
| `[InRange<Amount>]`      |   2.678 us |  0.07 |

## DefinedOnly & DefinedEnumOnly attribute

Based on a set of 1 milion (HTTP Status code) enum values.
| IsValid()           | Mean      | Ratio |
|---------------------|----------:|------:|
| `[DefinedEnumOnly]` | 10.666 ms |  1.00 |
| `[DefinedOnly<T>]`  |  5.061 ms |  0.47 |
| `[NotCached]`       | 47.058 ms |  4.42 |
| `[NotCached<T>]`    | 32.838 ms |  3.08 |

Based on a set of 1 milion flag enum values.

| IsValid()           | Mean      | Ratio |
|---------------------|----------:|------:|
| `[DefinedEnumOnly]` |  7.382 ms |  1.00 |
| `[DefinedOnly<T>]`  |  2.940 ms |  0.40 |

## Data Annotations Validation
| Method        | Runtime   | Count | Mean         | Error      | StdDev     | Median       | Ratio | RatioSD |
|-------------- |---------- |------ |-------------:|-----------:|-----------:|-------------:|------:|--------:|
| Qowaiv        | .NET 10.0 | 10    |     41.34 us |   1.927 us |   5.651 us |     42.00 us |  0.68 |    0.11 |
| Fluent        | .NET 10.0 | 10    |     35.86 us |   0.928 us |   2.524 us |     35.70 us |  0.59 |    0.06 |
| MiniValidator | .NET 10.0 | 10    |     60.92 us |   1.756 us |   4.954 us |     60.70 us |  1.00 |    0.00 |
|               |           |       |              |            |            |              |       |         |
| Qowaiv        | .NET 11.0 | 10    |     36.92 us |   1.730 us |   4.993 us |     36.35 us |  0.61 |    0.09 |
| Fluent        | .NET 11.0 | 10    |     36.35 us |   1.302 us |   3.651 us |     35.50 us |  0.60 |    0.07 |
| MiniValidator | .NET 11.0 | 10    |     60.63 us |   1.206 us |   3.280 us |     60.30 us |  1.00 |    0.00 |
|               |           |       |              |            |            |              |       |         |
| Qowaiv        | .NET 10.0 | 100   |     75.59 us |   1.511 us |   3.678 us |     75.45 us |  0.36 |    0.02 |
| Fluent        | .NET 10.0 | 100   |    135.56 us |   2.688 us |   5.114 us |    135.50 us |  0.64 |    0.03 |
| MiniValidator | .NET 10.0 | 100   |    211.32 us |   3.712 us |   5.557 us |    211.80 us |  1.00 |    0.00 |
|               |           |       |              |            |            |              |       |         |
| Qowaiv        | .NET 11.0 | 100   |     65.77 us |   1.308 us |   3.468 us |     66.10 us |  0.35 |    0.02 |
| Fluent        | .NET 11.0 | 100   |    111.30 us |   2.222 us |   6.121 us |    112.15 us |  0.60 |    0.04 |
| MiniValidator | .NET 11.0 | 100   |    186.26 us |   3.718 us |   7.163 us |    186.45 us |  1.00 |    0.00 |
|               |           |       |              |            |            |              |       |         |
| Qowaiv        | .NET 10.0 | 1000  |    391.97 us |   7.578 us |  11.572 us |    393.50 us |  0.28 |    0.01 |
| Fluent        | .NET 10.0 | 1000  |    968.49 us |  19.142 us |  35.482 us |    967.00 us |  0.70 |    0.03 |
| MiniValidator | .NET 10.0 | 1000  |  1,392.55 us |  27.591 us |  32.845 us |  1,398.60 us |  1.00 |    0.00 |
|               |           |       |              |            |            |              |       |         |
| Qowaiv        | .NET 11.0 | 1000  |    335.49 us |   6.321 us |   5.913 us |    337.00 us |  0.25 |    0.01 |
| Fluent        | .NET 11.0 | 1000  |    887.22 us |  17.576 us |  25.763 us |    883.80 us |  0.66 |    0.02 |
| MiniValidator | .NET 11.0 | 1000  |  1,349.83 us |  26.564 us |  24.848 us |  1,342.30 us |  1.00 |    0.00 |
|               |           |       |              |            |            |              |       |         |
| Qowaiv        | .NET 10.0 | 10000 |  4,398.47 us |  93.583 us | 274.463 us |  4,385.30 us |  0.27 |    0.02 |
| Fluent        | .NET 10.0 | 10000 | 10,214.84 us | 213.291 us | 608.533 us | 10,278.50 us |  0.62 |    0.04 |
| MiniValidator | .NET 10.0 | 10000 | 16,523.31 us | 326.459 us | 335.250 us | 16,579.80 us |  1.00 |    0.00 |
|               |           |       |              |            |            |              |       |         |
| Qowaiv        | .NET 11.0 | 10000 |  3,621.50 us |  71.697 us |  95.713 us |  3,631.00 us |  0.23 |    0.01 |
| Fluent        | .NET 11.0 | 10000 |  9,376.87 us | 185.748 us | 476.143 us |  9,201.20 us |  0.59 |    0.04 |
| MiniValidator | .NET 11.0 | 10000 | 15,996.99 us | 318.717 us | 712.855 us | 16,019.40 us |  1.00 |    0.00 |
