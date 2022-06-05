# Custom Single Value Object


## Validation
Ensuring the validness of a SVO is key. When using `Svo<TBehavior>` the
following mechanisms are provided:

### Setting the minimum and/or maximum length
By simply setting the `MinValue` and/or the `MaxValue` strings exceeding
these constrains are rejected:

``` C#
public sealed class ForMySvo : SvoBehavior
{
    public override int MinValue => 3;
    public override int MaxValue => 9;
}
```

### A regular expression pattern
If defined, the `Regex` pattern can act as second line of defense:

``` C#
public sealed class ForMySvo : SvoBehavior
{
    public override Regex Pattern => new("^()[A-Z][0-9])+$");
}
```

In this example by ensuring that all chars representing the SVO are alpha-numeric.

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
