namespace Qowaiv.Customization;

/// <summary>
/// Provides <see cref="string"/> based behavior for an identifier generated using
/// <see cref="IdAttribute{TBehavior, TRaw}"/>.
/// </summary>
[Inheritable]
public class StringIdBehavior : IdBehavior<string>
{
    /// <inheritdoc />
    [Pure]
    public override string FromBytes(byte[] bytes) => Encoding.ASCII.GetString(Guard.NotNull(bytes));

    /// <inheritdoc />
    [Pure]
    public override string NextId() => Uuid.NewUuid().ToString();

    /// <inheritdoc />
    [Pure]
    public override byte[] ToByteArray(string value) => Encoding.ASCII.GetBytes(Guard.NotNull(value));

    /// <inheritdoc />
    public sealed override bool TryTransform(string value, [NotNullWhen(true)] out string? transformed) => TryTransform(value, null, out transformed);

    /// <inheritdoc />
    public override bool TryTransform(string? str, IFormatProvider? formatProvider, out string? transformed)
    {
        transformed = str is { Length: > 0 } ? str : null;
        return true;
    }
}
