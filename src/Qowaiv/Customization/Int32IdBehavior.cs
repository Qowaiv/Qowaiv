namespace Qowaiv.Customization;

/// <summary>
/// Provides <see cref="int"/> based behavior for an identifier generated using
/// <see cref="IdAttribute{TBehavior, TRaw}"/>.
/// </summary>
[Inheritable]
public class Int32IdBehavior : IdBehavior<int>
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(int)
        || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(int)
        || base.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    [Pure]
    public sealed override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value) => value switch
    {
        int id when TryTransform(id, out var transformed) => transformed,
        string str when TryTransform(str, culture,  out var id) => id,
        _ => throw Exceptions.InvalidCast(value!.GetType(), SvoType),
    };

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => value is int id
        ? destinationType switch
        {
            var t when t == typeof(string) => ToString(id, null, culture),
            var t when t == typeof(int) => id,
            var t when t == typeof(long) => (long)id,
            _ => base.ConvertTo(context, culture, value, destinationType),
        }
        : base.ConvertTo(context, culture, value, destinationType);

    /// <inheritdoc />
    [Pure]
    public override int FromBytes(byte[] bytes) => BitConverter.ToInt32(bytes, 0);

    /// <inheritdoc />
    [Pure]
    public override byte[] ToByteArray(int id) => BitConverter.GetBytes(id);

    /// <inheritdoc />
    public override bool TryTransform(int value, [NotNullWhen(true)] out int transformed)
    {
        transformed = value;
        return value >= 0;
    }

    /// <inheritdoc />
    public override bool TryTransform(string? str, IFormatProvider? formatProvider, out int id)
    {
        if (str is not { Length: > 0 })
        {
            id = 0;
            return true;
        }
        else if (int.TryParse(str, NumberStyles.Integer, formatProvider, out var parsed)
           && TryTransform(parsed, out id))
        {
            return true;
        }
        id = default;
        return false;
    }
}
