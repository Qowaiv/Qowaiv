using Qowaiv.Sustainability;

namespace Qowaiv.Conversion.Sustainability;

/// <summary>Provides a conversion for an energy label.</summary>
[Inheritable]
public class EnergyLabelTypeConverter : SvoTypeConverter<EnergyLabel>
{
    /// <inheritdoc/>
    [Pure]
    protected override EnergyLabel FromString(string? str, CultureInfo? culture) => EnergyLabel.Parse(str, culture);
}
