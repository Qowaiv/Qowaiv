namespace Qowaiv.Customization;

/// <summary>
/// Provides <see cref="int"/> based behavior for an identifier generated using
/// <see cref="IdAttribute{TBehavior, TValue}"/>.
/// </summary>
[Inheritable]
public class Int32IdBehavior : IdBehavior<int>
{
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
        return value > 0;
    }

    /// <inheritdoc />
    public override bool TryTransform(string? str, IFormatProvider? formatProvider, out int id)
    {
        if (int.TryParse(str, NumberStyles.Integer, formatProvider, out var guid))
        {
            id = guid;
            return true;
        }
        id = default;
        return false;
    }
}
