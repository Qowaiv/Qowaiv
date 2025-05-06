#pragma warning disable S2436 // Types and methods should not have too many generic parameters
// Three is okay here.

namespace Qowaiv.Customization;

/// <summary>
/// Used by generated SVO's that uses the <see cref="CustomBehavior{TValue}"/>
/// to apply type conversions.
/// </summary>
/// <typeparam name="TSvo">
/// The type of the SVO.
/// </typeparam>
/// <typeparam name="TRaw">
/// The raw type representing the value of the SVO.
/// </typeparam>
/// <typeparam name="TConverter">
/// The type of the converter/behavior.
/// </typeparam>
public abstract class CustomBehaviorTypeConverter<TSvo, TRaw, TConverter> : TypeConverter
    where TSvo : struct
    where TConverter : CustomBehavior<TRaw>, new()
    where TRaw : IEquatable<TRaw>
{
    private static readonly TypeConverter Converter = new TConverter().WithSvoType(typeof(TSvo));

    /// <inheritdoc />
    [Pure]
    public sealed override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => Converter.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public sealed override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => Converter.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    [Pure]
    public sealed override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => FromRaw(Guard.IsInstanceOf<TRaw>(Converter.ConvertFrom(context, culture, value)));

    /// <inheritdoc />
    [Pure]
    public sealed override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => Converter.ConvertTo(context, culture, ToRaw(Guard.IsInstanceOf<TSvo>(value)), destinationType);

    /// <summary>Converts from the raw/underlying type.</summary>
    [Pure]
    protected abstract TSvo FromRaw(TRaw raw);

    /// <summary>Converts to the raw/underlying type.</summary>
    [Pure]
    protected abstract TRaw ToRaw(TSvo svo);
}
