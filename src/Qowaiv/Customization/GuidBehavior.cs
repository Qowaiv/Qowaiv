namespace Qowaiv.Customization;

/// <summary>
/// Provides <see cref="Guid"/> based behavior for an identifier generated using
/// <see cref="IdAttribute{TBehavior, TRaw}"/>.
/// </summary>
[Inheritable]
public class GuidBehavior : IdBehavior<Guid>
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(Guid)
        || sourceType == typeof(Uuid)
        || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(Guid)
        || destinationType == typeof(Uuid)
        || base.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    [Pure]
    public sealed override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value) => value switch
    {
        Guid guid when TryTransform(guid, out var transformed) => transformed,
        Uuid uuid when TryTransform(uuid, out var transformed) => transformed,
        string str when TryTransform(str, culture, out var id) => id,
        _ => throw Exceptions.InvalidCast(value.GetType(), SvoType),
    };

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => value is Guid guid
        ? destinationType switch
        {
            var t when t == typeof(Guid) => guid,
            var t when t == typeof(Uuid) => (Uuid)guid,
            var t when t == typeof(string) => ToString(guid, null, culture),
            _ => base.ConvertTo(context, culture, value, destinationType),
        }
        : base.ConvertTo(context, culture, value, destinationType);

    /// <inheritdoc />
    [Pure]
    public override Guid FromBytes(byte[] bytes) => new(bytes);

    /// <inheritdoc />
    [Pure]
    public override byte[] ToByteArray(Guid obj) => obj.ToByteArray();

    /// <inheritdoc />
    [Pure]
    public override Guid NextId() => Guid.NewGuid();

    /// <inheritdoc />
    public override bool TryTransform(Guid value, out Guid transformed)
    {
        transformed = value;
        return value != Guid.Empty;
    }

    /// <inheritdoc />
    public override bool TryTransform(string? str, IFormatProvider? formatProvider, out Guid id)
    {
        if (Uuid.TryParse(str, out var guid))
        {
            id = guid;
            return true;
        }
        id = Guid.Empty;
        return false;
    }
}
