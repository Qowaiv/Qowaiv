using Qowaiv.Globalization;

namespace Qowaiv.Conversion.Globalization;

/// <summary>Provides a conversion for a region.</summary>
[Inheritable]
public class RegionTypeConverter : SvoTypeConverter<Region>
{
    /// <inheritdoc/>
    [Pure]
    protected override Region FromString(string? str, CultureInfo? culture) => Region.Parse(str, culture);
}
