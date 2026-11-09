# Validation attributes
Multiple validation attributes can be used to decorate models.

## Mandatory
The `[Required]` attribute does not work for value types. The `[Mandatory]`
attribute does. The default value of the `struct` is not valid. It also is not
valid for the Unknown value, unless that is explicitly allowed.

``` C#
public class Model
{
    [Mandatory(AllowUnknownValue = true)]
    public EmailAddress Email { get; init; }

    [Mandatory()]
    public string SomeString { get; init; }
}
```

## Any
The `[Required]` attribute does not work (well) for collections. The `[Any]`
attribute does. It is only valid if the collection contains at least one item.

``` C#
public class Model
{
    [Any()]
    public List<int> Numbers { get; init; }
}
```

## Allowed values
The `[Allowed<TValue>]` attribute allows to define a subset of allowed values. It
supports type converters to get the allowed values based on a primitive value.

``` C#
public class Model
{
    [Allowed<Country>("DE", "FR", "GB")]
    public Country CountryOfBirth { get; init; }
}
```

## Forbidden values
The `[Forbidden<TValue>]` attribute allows to define a subset of forbidden values. It
supports type converters to get the forbidden values based on a primitive value.

``` C#
public class Model
{
    [Forbidden<Country>("US", "IR")]
    public Country CountryOfBirth { get; init; }
}
```

## Defined enum values only
The `[DefinedOnly<TEnum>]` attribute limits the allowed values of an enum to
the ones that are defined.

``` C#
public enum Status { Active = 1, Inactive = 2 }

public class Model
{
    [DefinedOnly<Status>]
    public Status Status { get; init; } // (Status)42 is not valid.
}
```

### Flag enums
By default, any combination of the defined values of a `[Flags]` enum is
valid, but values with bits that are not defined are not. That can be
restricted by setting `OnlyAllowDefinedFlagsCombinations` to `true`, in which
case only values that are explicitly defined are valid.

``` C#
[Flags]
public enum Permissions
{
    Read = 1,
    Write = 2,
    Delete = 4,
    ReadWrite = Read | Write,
}

public class Model
{
    // Read | Delete is valid; (Permissions)8 is not.
    [DefinedOnly<Permissions>]
    public Permissions Any { get; init; }

    // Read and ReadWrite are valid; Read | Delete is not.
    [DefinedOnly<Permissions>(OnlyAllowDefinedFlagsCombinations = true)]
    public Permissions Defined { get; init; }
}
```

The default value `0` is valid for flag enums by default, even if no member
with that value is defined. With `OnlyAllowDefinedFlagsCombinations = true` it
is only valid if it is defined (for example `None = 0`).
`OnlyAllowDefinedFlagsCombinations` has no effect on enums that are not
decorated with `[Flags]`.

### Non-generic
If generic attributes are not supported ([pre-.NET 7.0/C#11](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-7)),
the non-generic `[DefinedEnumValuesOnly]` attribute can be used. It behaves the
same, also supporting `OnlyAllowDefinedFlagsCombinations`.

``` C#
public class Model
{
    [DefinedEnumValuesOnly(OnlyAllowDefinedFlagsCombinations = true)]
    public Permissions Permissions { get; init; }
}
```

Both attributes throw an `UnsupportedType` exception when applied to a member
whose type is not an enum, instead of reporting a validation error.

## In future
The `[InFuture]` attribute requires a `DateTime`, `DateTimeOffset`, `Date`,
`DateOnly`, or `Year` value to be in the future. The current time is resolved
using [`Qowaiv.Clock.UtcNow()`](https://github.com/Qowaiv/Qowaiv/blob/master/README.md#qowaiv-clock).

``` C#
public class Model
{
    [InFuture]
    public DateOnly? ExpiryDate { get; init; }
}
```

## In past
The `[InPast]` attribute requires a `DateTime`, `DateTimeOffset`, `Date`,
`DateOnly`, or `Year` value to be in the past. The current time is resolved
using [`Qowaiv.Clock.UtcNow()`](https://github.com/Qowaiv/Qowaiv/blob/master/README.md#qowaiv-clock).

``` C#
public class Model
{
    [InPast]
    public DateOnly DateOfBirth { get; init; }
}
```

## In range
The `[InRange<TValue>]` attribute limits the allowed values to the specified range.
This attribute is similar to Microsoft's [`Range`](https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.rangeattribute).

``` C#
public class Model
{
    [InRange<int>(1, 10)]
    public int Rating { get; init; }
}
```

## Items validation
The `[Items<TValidator>]` attribute defines a validation attribute to apply
on all items of a collection. This is useful in multiple cases:

``` C#
public class Model
{
    [Items<Mandatory>] // ensures none of the items is null or empty
    public string[] Names { get; init; } = [];

    [Items<Allowed<int>>(42, 2017)] // ensures that all items have either the value 42 or 2017.
    public int[] Numbers { get; init; } = [];
}
```

By defining either `ErrorMessage` or both `ErrorMessageResourceName` and `ErrorMessageResourceType`,
those values are set to the `TValidator` allowing full control of the error message generation.

## Is finite
The `[IsFinite]` attribute validates that the floating point value of the field
represents a finite (i.e. not NaN, or infinity).

``` C#
public class Model
{
    [IsFinite]
    public double Number { get; init; }
}
```

## Skip validation
The `[SkipValidation]` attribute allows to skip the validation of a property
or type.

``` C#
[SkipValidation]
public class Model
{
    [SkipValidation]
    public double Number { get; init; }
}
```

## Multiple of
The `[MultipleOf]` attribute validates that the value of a field is a multiple
of the specified factor. It supports types such as `decimal`, `Percentage`, and
`Amount`, but not `double` or `float`, as their [decimal representation](https://0.30000000000000004.com)
does not commonly fit with decimal based multiples such as `0.1`, or `0.001`.
Note that the specified factor in the attribute is a `double` but rounded to
overcome those floating point issues.

``` C#
public class Model
{
    [MultipleOf(0.001)]
    public Amount Total { get; init; }
}
```

## Negative
The `[Negative]` attribute validates that the numeric value of a field is negative
(less than zero).

``` C#
public class Model
{
    [Negative]
    public int Temperature { get; init; }

    [Negative]
    public Amount Balance { get; init; }
}
```

## Not negative
The `[NotNegative]` attribute validates that the numeric value of a field is not negative
(greater than or equal to zero).

``` C#
public class Model
{
    [NotNegative]
    public int Count { get; init; }

    [NotNegative]
    public Amount Value { get; init; }
}
```

## Not positive
The `[NotPositive]` attribute validates that the numeric value of a field is not positive
(less than or equal to zero).

``` C#
public class Model
{
    [NotPositive]
    public int Adjustment { get; init; }

    [NotPositive]
    public Amount Discount { get; init; }
}
```

## Positive
The `[Positive]` attribute validates that the numeric value of a field is positive
(greater than zero).

``` C#
public class Model
{
    [Positive]
    public int Quantity { get; init; }

    [Positive]
    public Amount Discount { get; init; }
}
```

## Not in future
The `[NotInFuture]` attribute requires a `DateTime`, `DateTimeOffset`, `Date`,
`DateOnly`, or `Year` value not to be in the future. The current time is resolved
using [`Qowaiv.Clock.UtcNow()`](https://github.com/Qowaiv/Qowaiv/blob/master/README.md#qowaiv-clock).

``` C#
public class Model
{
    [NotInFuture]
    public DateTime CreationTime { get; init; }
}
```

## Not in past
The `[NotInPast]` attribute requires a `DateTime`, `DateTimeOffset`, `Date`,
`DateOnly`, or `Year` value not to be in the past. The current time is resolved
using [`Qowaiv.Clock.UtcNow()`](https://github.com/Qowaiv/Qowaiv/blob/master/README.md#qowaiv-clock).

``` C#
public class Model
{
    [NotInPast]
    public DateOnly Start { get; init; }
}
```

## Not unknown
The `[NotUnknown]` attribute indicates that the field does not allow the unknown value.

``` C#
public class Model
{
    [NotUnknown]
    public EmailAddress Address { get; init; }
}
```

## Optional
The `[Optional]` attribute indicates explicitly that a field is optional.

``` C#
public class Model
{
    [Optional]
    public string? Message { get; init; }
}
```

## Unique values
The `[Unique<TValue>]` attribute validates that all items of the collection are
distinct. If needed, a custom `IEqualityComparer<TValue>` can be defined.

``` C#
public class Model
{
    [Unique<int>(typeof(CustomEqualityComparer))]
    public IEnumerable<int> Numbers { get; init; }
}
```

## Collection size
The `[Collection.AtLeast]`, `[Collection.AtMost]`, and `[Collection.InRange]`
attributes validate the number of items in a collection.

``` C#
public class Model
{
    [Collection.AtLeast(1)]
    public List<string> Items { get; init; } = [];

    [Collection.AtMost(10)]
    public List<int> Numbers { get; init; } = [];

    [Collection.InRange(1, 5)]
    public string[] Tags { get; init; } = [];
}
```

## Length (string/array)
The `[Length.AtLeast]`, `[Length.AtMost]`, and `[Length.InRange]` attributes
validate the length of strings or arrays.

``` C#
public class Model
{
    [Length.AtLeast(1)]
    public string Name { get; init; }

    [Length.AtMost(255)]
    public string Description { get; init; }

    [Length.InRange(2, 10)]
    public string Code { get; init; }
}
```

## Size (streams and binary data)
The `[Size.AtLeast]`, `[Size.AtMost]`, and `[Size.InRange]` attributes
validate the size of streams and binary data collections. They support
`Stream`, `ICollection<byte>`, `IReadOnlyCollection<byte>`, and `BinaryData`.

``` C#
public class Model
{
    [Size.AtMost(1024 * 1024)] // max 1 MB
    public Stream Upload { get; init; }

    [Size.InRange(16, 64)]
    public BinaryData Data { get; init; }
}
```

## Validates attribute
The `[Validates]` attribute is designed to help the [QW0102](https://github.com/Qowaiv/qowaiv-analyzers/blob/main/rules/QW0102.md)
to report on misusage of the attribute. By decorating a `ValidationAttribute`
the analyzer knows on which member types the attribute can be set, and report
an issue when used on a field of an not supported type. Note that the attribute
is decorated with `[Conditional("CONTRACTS_FULL")]` so unless `CONTRACTS_FULL`
is set during the build, that argument only acts as contract annotation.

``` C#
[Validates(typeof(string))]
public sealed class ContainsTextAttribute(string text) : ValidationAttribute 
{
    // ...
}
```

``` C#
[Validates(GenericArgument = true)]
public sealed class DefinedOnlyAttribute<TEnum> : ValidationAttribute where TEnum : struct, Enum
{
    // ..
}
```
