namespace Qowaiv.Customization;

/// <summary>
/// Provides <see cref="long"/> based behavior for an identifier generated using
/// <see cref="IdAttribute{TBehavior, TRaw}"/>.
/// </summary>
[Inheritable]
public class Int64IdBehavior : IdBehavior<long>
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(int)
        || sourceType == typeof(long)
        || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(int)
        || destinationType == typeof(long)
        || base.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    [Pure]
    public sealed override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value) => value switch
    {
        int id when TryTransform(id, out var transformed) => transformed,
        long id when TryTransform(id, out var transformed) => transformed,
        string str when TryTransform(str, culture, out var id) => id,
        _ => throw Exceptions.InvalidCast(value.GetType(), SvoType),
    };

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => AsLong(value) is long id
        ? destinationType switch
        {
            var t when t == typeof(string) => ToString(id, null, culture),
            var t when t == typeof(int) => (int)id,
            var t when t == typeof(long) => id,
            _ => base.ConvertTo(context, culture, value, destinationType),
        }
        : base.ConvertTo(context, culture, value, destinationType);

    [Pure]
    private static object? AsLong(object? value) => value switch { int i => (long)i, long l => l, _ => null };

    /// <inheritdoc />
    [Pure]
    public override long FromBytes(byte[] bytes) => BitConverter.ToInt64(bytes, 0);

    /// <inheritdoc />
    [Pure]
    public override byte[] ToByteArray(long id) => BitConverter.GetBytes(id);

    /// <inheritdoc />
    public override bool TryTransform(long value, [NotNullWhen(true)] out long transformed)
    {
        transformed = value;
        return value >= 0;
    }

    /// <inheritdoc />
    public override bool TryTransform(string? str, IFormatProvider? formatProvider, out long id)
    {
        if (str is not { Length: > 0 })
        {
            id = 0;
            return true;
        }
        else if (long.TryParse(str, NumberStyles.Integer, formatProvider, out id) && id >= 0)
        {
            return true;
        }
        id = default;
        return false;
    }
}
