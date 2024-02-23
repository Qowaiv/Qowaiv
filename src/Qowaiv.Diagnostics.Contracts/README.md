# Qowaiv Diagnostics Contracts
This packages contains attributes to define (expected) behavior on code.

Most likely, you only need this dependency compile time. In that case you
should add the dependency as follows:

``` XML
<ItemGroup>
  <PackageReference
    Include="Qowaiv.Diagnostics.Contracts"
    Version="*"
    PrivateAssets=""all"
    IncludeAssets="compile" />
<ItemGroup>
```

To embed the result of these decorations in the compiled assembly, the `DEFINE`
constant `CONTRACTS_FULL` has to be set.

``` XML
<PropertyGroup>
  <DefineConstants>CONTRACTS_FULL</DefineConstants>
</PropertyGroup>
```

If you only use these decorations to enable static code analysis, this constant
is not needed.

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

## Empty type attributes
Empty types (classes, enums, interfaces, structs) are generally seen as a bad
practice, or assumed to be unfinished code. Decorating them with an attribute
that explains the reason why the type is empty (for test cases for example)
can help:

* `[EmptyClass(Justification = "")]`
* `[EmptyEnum(Justification = "")]`
* `[EmptyInterface(Justification = "")]`
* `[EmptyStruct(Justification = "")]`
* `[EmptyTestClass]`
* `[EmptyTestEnum]`
* `[EmptyTestInterface]`
* `[EmptyTestStruct]`


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
