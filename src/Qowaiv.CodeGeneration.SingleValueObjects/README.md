# Custom Single Value Object
Creating SVO's for the scope of (just) a project, might feel like a burden that
is not worth the effort. To reduce the burden, this package comes with a code
generator:

``` C#
[Svo<CustomBehavior>]
public partial readonly struct CustomSvo
{
    private sealed class CustomBehavior : SvoBehavior { /* .. */ }
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
private sealed class CustomBehavior : SvoBehavior
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
private sealed class CustomBehavior : SvoBehavior
{
    public override Regex Pattern => new("^([A-Z][0-9])+$");
}
```

### Overriding the validation method
For more complex scenarios, the first options provided might not be sufficient.
Therefore, is also possible to override `IsValid()`:

``` C#
private sealed class CustomBehavior : SvoBehavior
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
private sealed class CustomBehavior : SvoBehavior
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
private sealed class CustomBehavior : SvoBehavior
{
    public override string FormatUnknown(string? format, IFormatProvider? formatProvider) 
        => "Unknown";

    public override string Format(string str, string? format, IFormatProvider? formatProvider)
        => format == "l"
        ? str.ToLower()
        : str;
}
```
