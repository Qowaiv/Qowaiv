# Contracts

## Enumerated
[Enumerated types](https://en.wikipedia.org/wiki/Enumerated_type) are sets of
named values. In domain-driven design it is common to create types that have
extended functionality compared to the built-in [primitive enums](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/enum).

The `Qowaiv.IEnumerated<TSelf>` interface allows both to detect SVOs that act
as (smart) enumerated types, and to get all defined values.

```csharp
var sexes = Sex.DefinedValues; // Male, Female, NotApplicable.
```

### Empty and unknown
The empty and unknown values are not part of the defined values. Both express
the absence of information, and are to a large extent equivalent. They are,
however, (potentially) valid input, so they are not ignored everywhere:

| Source                                | Contains                                  |
|---------------------------------------|-------------------------------------------|
| `DefinedValues`                       | Only the values with domain meaning       |
| `OpenApiDataType.Enum`                | Defined values + the unknown value        |

For types that do not specify the `enum` of their `[OpenApiDataType]` explicitly,
`OpenApiDataType.FromType()` resolves it from `DefinedValues`, followed by the
unknown value (when the type has one).
