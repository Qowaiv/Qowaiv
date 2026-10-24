# Contracts

## Enumerated
[Enumerated types](https://en.wikipedia.org/wiki/Enumerated_type) are set of
named values. In domain-driven design it is common to create types that have
extended functionality compared to the built-in [primtive enums](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/enum).

The `Qowaiv.IEnumerated<TSelf>` interface allows both to detect SVOs that act
as (smart) enumerated types, and to get all defined enums.

```csharp
var sexes = Sex.DefinedValues; // Male, Female, NotApplicable.
```
