namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a Yes-no.</summary>
public class SvoConverter : SvoTypeConverter<Svo>
{
    /// <inheritdoc/>
    [Pure]
    protected override Svo FromString(string? str, CultureInfo? culture) => Svo.Parse(str, culture);
}
