# Qowaiv Diagnostics Contracts
This packages contains attributes to define (expected) behavior on code.

## Impure attribute
Opposed to the `[Pure]` attribute, the `[Impure]` attribute indicates that a
method has side effects. This attribute can help working with static code 
analyzer rule: [QW0003](https://github.com/Qowaiv/qowaiv-analyzers/blob/main/rules/QW0003.md).

### Collection mutation attribute
An attribute that inherits from `[Impure]` to indicate that the collection
changes due to this method call.

### Fluent syntax mutation attribute
An attribute that inherits from `[Impure]` to indicate that the returned
instance is equal to self or of the parameters, just to allow a fluent syntax.

## Inheritable attribute
The `[Inheritable]` attribute indicates that a class is designed to be
inheritable although no virtual or protected members have been defined.
This attribute can help working with static code analyzer rule: [QW0006](https://github.com/Qowaiv/qowaiv-analyzers/blob/main/rules/QW0006.md).

### Will be sealed attribute
An attribute that inherits from `[Inheritable]` that indicates that the
decorated member will be sealed in the future, and that overrides are considered
obsolete code.

## Mutable attribute
Indicates that a class, record, interface or struct is mutable by design.
This attribute can help working with static code analyzer rules:
[QW0011](https://github.com/Qowaiv/qowaiv-analyzers/blob/main/rules/QW0011.md)
and [QW0012](https://github.com/Qowaiv/qowaiv-analyzers/blob/main/rules/QW0012.md).
