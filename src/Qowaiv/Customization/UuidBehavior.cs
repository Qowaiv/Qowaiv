namespace Qowaiv.Customization;

/// <summary>
/// Provides <see cref="Uuid"/> based behavior for an identifier generated using
/// <see cref="IdAttribute{TBehavior, TValue}"/>.
/// </summary>
[Inheritable]
public class UuidBehavior : IdBehavior<Uuid>
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
        null or "" => Uuid.Empty,
        Guid guid => (Uuid)guid,
        Uuid uuid => uuid,
        string str when Uuid.TryParse(str, out var id) => id,
        _ => throw Exceptions.InvalidCast(value.GetType(), typeof(Uuid)),
    };

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => value is Uuid uuid
        ? destinationType switch
        {
            var t when t == typeof(Guid) => (Guid)uuid,
            var t when t == typeof(Uuid) => uuid,
            var t when t == typeof(string) => uuid.ToString(),
            _ => base.ConvertTo(context, culture, value, destinationType),
        }
        : base.ConvertTo(context, culture, value, destinationType);

    /// <inheritdoc />
    [Pure]
    public override Uuid FromBytes(byte[] bytes) => new Guid(bytes);

    /// <inheritdoc />
    [Pure]
    public override byte[] ToByteArray(Uuid obj) => obj.ToByteArray();

    /// <inheritdoc />
    [Pure]
    public override Uuid NextId() => Uuid.NewUuid();

    /// <inheritdoc />
    public override bool TryTransform(Uuid value, out Uuid transformed)
    {
        transformed = value;
        return value != Uuid.Empty;
    }

    /// <inheritdoc />
    public override bool TryTransform(string? str, IFormatProvider? formatProvider, out Uuid id)
    {
        if (Uuid.TryParse(str, out var guid))
        {
            id = guid;
            return true;
        }
        id = Uuid.Empty;
        return false;
    }
}
