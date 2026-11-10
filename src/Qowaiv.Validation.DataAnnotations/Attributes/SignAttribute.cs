using Qowaiv.Financial;
using Qowaiv.IO;
using Qowaiv.Mathematics;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Base attributes for validating members in comparision to zero..</summary>
[CLSCompliant(false)]
public abstract class SignAttribute(Func<string> errorMessageAccessor) : ValidationAttribute(errorMessageAccessor)
{
    /// <inheritdoc />
    [Pure]
    [SuppressMessage("Critical Code Smell", "S1541:Methods and properties should not be too complex", Justification = "Straightforward switch")]
    public sealed override bool IsValid(object? value) => value switch
    {
        null => true,
        sbyte v /*......*/ => Is(Math.Sign(v)),
        short v /*......*/ => Is(Math.Sign(v)),
        int v /*........*/ => Is(Math.Sign(v)),
        long v /*.......*/ => Is(Math.Sign(v)),
        float v /*......*/ => !float.IsNaN(v) && Is(Math.Sign(v)),
        double v /*.....*/ => !double.IsNaN(v) && Is(Math.Sign(v)),
        decimal v /*....*/ => Is(Math.Sign(v)),
        TimeSpan v /*...*/ => Is(Math.Sign(v.Ticks)),
#if NET8_0_OR_GREATER
        Int128 v /*.....*/ => Is(Int128.Sign(v)),
#endif
        Amount v /*.....*/ => Is(v.Sign()),
        Fraction v /*...*/ => Is(v.Sign()),
        Money v /*......*/ => Is(v.Sign()),
        MonthSpan v /*..*/ => Is(v.Sign()),
        Percentage v /*.*/ => Is(v.Sign()),
        StreamSize v /*.*/ => Is(v.Sign()),
        YearSpan v /*...*/ => Is(v.Sign()),
        _ => throw UnsupportedType.ForAttribute<SignAttribute>(value.GetType()),
    };

    /// <summary>Validates if the sign is matching the attribute constraint.</summary>
    [Pure]
    protected abstract bool Is(int sign);
}
