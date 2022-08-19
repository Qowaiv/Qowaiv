# Custom Single Value Object
Creating SVO's for the scope of (just) a project, might feel like a burden that
is not worth the effort. Although Qowaiv has as [code generator](https://github.com/Qowaiv/qowaiv-codegenerator),
it still results in quite some lines of code (easily 400), that needs
maintenance, and unit test coverage.

Another way to deal with this, is by providing a generic SVO where the actual
behavior is injectable. Qowaiv has two flavors: `Id<TIdBehavior>` and
`Svo<TSvoBehavior>`. In this document we focus on the latter. 

``` C#
public sealed class ForMyCustomSvo : SvoBehavior { /* .. */ }

public class MyModel
{
    public Svo<ForMyCustomSvo> MyProperty { get; init; }
}
```

With (global) `using` statements, this can be improved a lot:

``` C#
global using MyCustomSvo = Qowaiv.Svo<ForMyCustomSvo>;

public class MyModel
{
    public MyCustomSvo MyProperty { get; init; }
}
```

Just having this definition might prevent some primitive obsession, but
with implementing some custom behavior, we can achieve some nice results.


## Validation
Ensuring the validness of a SVO is key. The following mechanisms are provided:

### Setting the minimum and/or maximum length
By simply setting the `MinLength` and/or the `MaxLength` strings exceeding
these constrains are rejected:

``` C#
public sealed class ForMySvo : SvoBehavior
{
    public override int MinLength => 3;
    public override int MaxLength => 9;
}
```

### A regular expression pattern
If defined, the `Regex` pattern can act as a second line of defense. As in the
following example, where we ensure that all characters that make up the SVO are
alphanumeric:

``` C#
public sealed class ForMySvo : SvoBehavior
{
    public override Regex Pattern => new("^([A-Z][0-9])+$");
}
```

### Overriding the validation method
For more complex scenarios, the first options provided might not be sufficient.
Therefore, is also possible to override `IsValid()`:

``` C#
public sealed class ForMySvo : SvoBehavior
{
    public override bool IsValid(string str, IFormatProvider? formatProvider, out string validated)
    {
        if (long.TryParse(str, out var number))
        {
            validated = number.ToString("000");
            return true;
        }
        else
        {
            validated = default;
            return false;
        }
    }
}
```

### Normalizing input
Before the input string is validated, the `NormalizeInput()` method is called.
By default the input is trimmed, but if preferred, extra measures can be taken:

```  C#
public sealed class ForMySvo : SvoBehavior
{
    public override string NormalizeInput(string str, IFormatProvider formatProvider) 
        => str?.Replace("-", "").ToUpperInvariant()
        ?? string.Empty;
}
```

## Formatting
By default the formatting is simple: `string.Empty` for the empty state, `"?"`
for the unknown state, and the internally stored string for the rest. The empty
state behavior is not overridable, the other two are:

```  C#
public sealed class ForMySvo : SvoBehavior
{
    public override string FormatUnknown(string? format, IFormatProvider? formatProvider) 
        => "Unknown";

    public override string Format(string str, string? format, IFormatProvider? formatProvider)
        => format == "l"
        ? str.ToLower()
        : str;
}
```

## Limitations
It should be pointed out that this approach has its limitations. Defining extra
properties on a `Svo<TSvoBehavior>` is not possible, neither are casts,
operation overloads or factory methods. Extra methods are, but only as extension
methods.
