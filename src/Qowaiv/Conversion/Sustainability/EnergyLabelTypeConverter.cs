using Qowaiv.Sustainability;

namespace Qowaiv.Conversion.Sustainability;

/// <summary>Provides a conversion for a Sex.</summary>
[Inheritable]
public class EnergyLabelTypeConverter : SvoTypeConverter<EnergyLabel>
{
    /// <inheritdoc/>
    [Pure]
    protected override EnergyLabel FromString(string? str, CultureInfo? culture) => EnergyLabel.Parse(str, culture);
}
