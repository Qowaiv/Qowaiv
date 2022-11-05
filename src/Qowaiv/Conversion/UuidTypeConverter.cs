namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a GUID.</summary>
[Inheritable]
public class UuidTypeConverter : SvoTypeConverter<Uuid, Guid>
{
    /// <inheritdoc />
    [Pure]
    protected override Uuid FromString(string? str, CultureInfo? culture) => Uuid.Parse(str);

    /// <inheritdoc />
    [Pure]
    protected override Uuid FromRaw(Guid raw) => raw;

    /// <inheritdoc />
    [Pure]
    protected override Guid ToRaw(Uuid svo) => svo;
}
