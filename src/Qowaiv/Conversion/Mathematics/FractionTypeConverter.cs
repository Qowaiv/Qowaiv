using Qowaiv.Mathematics;

namespace Qowaiv.Conversion.Mathematics;

/// <summary>Provides a conversion for an email address.</summary>
[Inheritable]
public class FractionTypeConverter : SvoTypeConverter<Fraction>
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => IsNumber(sourceType)
        || base.CanConvertFrom(context, sourceType);

    [Pure]
    private static bool IsNumber(Type sourceType) 
        => sourceType == typeof(decimal)
        || sourceType == typeof(int)
        || sourceType == typeof(long)
        || sourceType == typeof(double);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value) => value switch
    {
        int int32 => Fraction.Create(int32),
        long int64 => Fraction.Create(int64),
        decimal dec => Fraction.Create(dec),
        double dbl => Fraction.Create(dbl),
        _ => base.ConvertFrom(context, culture, value),
    };

    /// <inheritdoc/>
    [Pure]
    protected override Fraction FromString(string? str, CultureInfo? culture) => Fraction.Parse(str, culture);
}
