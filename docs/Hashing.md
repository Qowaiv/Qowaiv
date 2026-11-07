# Hashing
To support hashing (`object.GetHashCode()`) the hash code should always return
the same value for the same object. As SVOs are equal by value, the hash is
calculated based on the underlying value.

For security measures, however, this is only true within the same app domain.
By having different hashes for the same value in different app domains, there
is a good defense against hash flooding.

`Hash.Code()` also supports a fluent syntax, to get hash codes for complex
objects:
``` C#
public int GetHashCode() => Hash
    .Code(Prop)
    .And(Other)
    .And(Collection); // takes all items into account
```

This works _out-of-the-box_ because `Hash` can be implicitly cast to an `int`.
Calling `Hash.GetHashCode()` is not allowed (it throws `HashingNotSupported`),
just use the implicit cast.

## Testing
To assert that `.GetHashCode()` is value-based and stable, use
[`HashCodes.For(..).Should().BeConsistent()`](test-tools/HashCodes.md).
