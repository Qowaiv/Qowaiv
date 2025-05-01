namespace Qowaiv.Customization;

/// <summary>
/// Provides <see cref="long"/> based behavior for an identifier generated using
/// <see cref="IdAttribute{TBehavior, TValue}"/>.
/// </summary>
[Inheritable]
public class Int64IdBehavior : IdBehavior<long>
{
    /// <inheritdoc />
    [Pure]
    public override long FromBytes(byte[] bytes) => BitConverter.ToInt64(bytes, 0);

    /// <inheritdoc />
    [Pure]
    public override byte[] ToByteArray(long value) => BitConverter.GetBytes(value);

    /// <inheritdoc />
    public override bool TryTransform(long value, [NotNullWhen(true)] out long transformed)
    {
        transformed = value;
        return value > 0;
    }

    /// <inheritdoc />
    public override bool TryTransform(string? str, IFormatProvider? formatProvider, out long id)
    {
        if (long.TryParse(str, NumberStyles.Integer, formatProvider, out var guid))
        {
            id = guid;
            return true;
        }
        id = default;
        return false;
    }
}
