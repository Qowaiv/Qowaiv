using Qowaiv.Reflection;

namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for numeric types.</summary>
/// <typeparam name="TSvo">
/// The type of the SVO.
/// </typeparam>
/// <typeparam name="TRaw">
/// The type that represents the raw (primitive) value of the SVO.
/// </typeparam>
public abstract class NumericTypeConverter<TSvo, TRaw> : SvoTypeConverter<TSvo, TRaw>
    where TSvo : struct, IFormattable
    where TRaw : struct, IFormattable
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => IsConvertable(sourceType) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => IsConvertable(destinationType) || base.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        if (value is null)
        {
            return Activator.CreateInstance<TSvo>();
        }
        else if (IsConvertable(value.GetType()))
        {
            var raw = (TRaw)Convert.ChangeType(value, typeof(TRaw));
            return FromRaw(raw);
        }
        else return base.ConvertFrom(context, culture, value);
    }

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        Guard.NotNull(destinationType);

        // If the value is null or default value.
        if (QowaivType.IsNullOrDefaultValue(value))
        {
            return QowaivType.IsNullable(destinationType) ? null : Activator.CreateInstance(destinationType);
        }
        else if (IsConvertable(destinationType))
        {
            var svo = Guard.IsInstanceOf<TSvo>(value);
            var raw = ToRaw(svo);
            return Convert.ChangeType(raw, QowaivType.GetNotNullableType(destinationType));
        }
        else return base.ConvertTo(context, culture, value, destinationType);
    }

    /// <summary>Returns true if the conversion is supported.</summary>
    [Pure]
    protected virtual bool IsConvertable(Type? type)
        => type is not null
        && type != typeof(TSvo)
        && QowaivType.IsNumeric(type);
}
