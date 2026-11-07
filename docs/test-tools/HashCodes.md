# Hash Code assertions
`HashCodes` is a helper to test that [`GetHashCode()`](https://learn.microsoft.com/en-us/dotnet/api/system.object.gethashcode)
is implemented correctly: the same value should always produce the same hash
code. As hash codes might be different per app domain and different TFMs might
hash different primitives differently, asserting on there `int` value might
**not** be practical.

## Usage
Using the helper looks like this:

``` C#
[Test]
public void hash_code_is_value_based() => HashCodes
    .For(Country.All).Should().BeConsistent();
```

or
``` C#
[Test]
public void hash_code_is_value_based() => HashCodes
    .For(12.0m.Percent(), 1.3m.Percent(), 0.Percent()).Should().BeConsistent();
```
It is asserted that:
1. all supplied values are unique,
2. each value returns the same hash code on every call of `GetHashCode()`.
