# Benchmarks

## Event Buffer
The `Qowaiv.DomainModel.EventBuffer<T>` is an append-only immutable collection.
In this set of benchmarks, its compared with `System.Collections.Generic.List<object>`.
As the event buffer ensures `null` values are not added, for fairness, code is
added to the list benchmarks that check for nullability too.

### Creation
By making both the event buffer and its only underlying append only collection
structs the overhead of object allocation is minimized. Locks and null checks
still take time.

Note that **list**:
``` C#
foreach (var e in Events)
{
    if (e is not null)
    {
        list.Add(e);
    }
}
```

is significantly faster then **where clause**:
``` C#
foreach (var e in Events.Where(e => e is { }))
{
    list.Add(e);
}
```

|       Method |   Count |         Mean | Ratio |       Gen0 |     Gen1 |     Gen2 |    Allocated |
|-------------:|- ------:|-------------:|------:|-----------:|---------:|---------:|-------------:|
|         list |   1,000 |     8.700 µs |  1.00 |     1.3123 |        - |        - |     16.24 KB |
| where clause |   1,000 |    14.722 µs |  1.67 |     1.3123 |   0.0305 |        - |      16.3 KB |
| event buffer |   1,000 |    27.622 µs |  3.17 |     1.3123 |   0.0305 |        - |     16.16 KB |
|  previous[1] |   1,000 |     9.373 µs |  1.07 |     5.7373 |   0.3357 |        - |     70.35 KB |
|              |         |              |       |            |          |          |              |
|         list |  10,000 |   121.215 µs |  1.00 |    41.6260 |  41.6260 |  41.6260 |    256.35 KB |
| where clause |  10,000 |   184.068 µs |  1.54 |    41.5039 |  41.5039 |  41.5039 |     256.4 KB |
| event buffer |  10,000 |   293.248 µs |  2.42 |    41.5039 |  41.5039 |  41.5039 |    256.26 KB |
|  previous[1] |  10,000 |   106.066 µs |  0.87 |    57.3730 |  24.2920 |        - |    703.16 KB |
|              |         |              |       |            |          |          |              |
|         list | 100,000 | 1,142.454 µs |  1.00 |   367.1875 | 357.4219 | 357.4219 |  2,048.52 KB |
| where clause | 100,000 | 1,775.404 µs |  1.48 |   468.7500 | 458.9844 | 458.9844 |  2,048.62 KB |
| event buffer | 100,000 | 2,774.984 µs |  2.42 |   503.9063 | 496.0938 | 496.0938 |  2,048.47 KB |
|  previous[1] | 100,000 | 3,013.979 µs |  2.64 |   570.3125 | 425.7813 |        - |  7,031.29 KB |
|              |         |              |       |            |          |          |              |
|         list | 200,000 | 2,406.188 µs |  1.00 |   507.8125 | 500.0000 | 500.0000 |  4,096.72 KB |
| where clause | 200,000 | 3,527.164 µs |  1.44 |   605.4688 | 597.6563 | 597.6563 |  4,096.72 KB |
| event buffer | 200,000 | 5,854.206 µs |  2.42 |   492.1875 | 484.3750 | 484.3750 |  4,096.48 KB |
|  previous[1] | 200,000 | 9,733.000 µs |  4.04 | 1,140.6250 | 984.3750 |        - | 14,062.55 KB |

### Batch creation
For batch creation, in the list example `Events.Where(e => e { })` is chosen.
As you can see, that is slower than adding item per item with the if-statement.
This is due to the fact list now can not rely on `ICollection<T>.CopyTo(T[], int)`.

|       Method |   Count |         Mean | Ratio |       Gen0 |     Gen1 |     Gen2 |    Allocated |
|-------------:|--------:|-------------:|------:|-----------:|---------:|---------:|-------------:|
|         List |   1,000 |    15.104 µs |  1.00 |     1.3275 |   0.0458 |        - |     16.30 KB |
| Event Buffer |   1,000 |     8.965 µs |  0.60 |     1.3123 |   0.0305 |        - |     16.16 KB |
|              |         |              |       |            |          |          |              |
|         List |  10,000 |   187.949 µs |  1.00 |    41.5039 |  41.5039 |  41.5039 |    256.40 KB |
| Event Buffer |  10,000 |   108.499 µs |  0.57 |    41.6260 |  41.6260 |  41.6260 |    256.26 KB |
|              |         |              |       |            |          |          |              |
|         List | 100,000 | 1,848.881 µs |  1.00 |   484.3750 | 474.6094 | 474.6094 |  2,048.60 KB |
| Event Buffer | 100,000 | 1,162.800 µs |  0.63 |   330.0781 | 320.3125 | 320.3125 |  2,048.44 KB |

### Iteration
The event buffer can be looped faster than a list, and almost as fast as an
array. This is possible as the immutable event buffer does not have the check
for changes, where a list has too.

|              |   1,000 |   10,000 |    100,000 |
|-------------:|--------:|---------:|-----------:|
|        array | 11.2 µs | 107.5 µs | 1.071,9 µs |
| event buffer | 12.7 µs | 121.4 µs | 1.211,0 µs |
|         list | 15.4 µs | 148.9 µs | 1.501,9 µs |
|    aggregate | 20.5 µs | 196.0 µs | 1.994,8 µs |
|  previous[1] | 36.4 µs | 507.7 µs |     N/A    |

1. `EventBuffer` of v0.2.0.
