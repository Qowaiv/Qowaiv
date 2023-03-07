using Qowaiv.Statistics;

namespace Qowaiv.Conversion.Statistics;

/// <summary>Provides a conversion for an Elo.</summary>
[Inheritable]
public class EloTypeConverter : NumericTypeConverter<Elo, double>
{
    /// <inheritdoc/>
    [Pure]
    protected override Elo FromRaw(double raw) => raw;

    /// <inheritdoc/>
    [Pure]
    protected override Elo FromString(string? str, CultureInfo? culture) => Elo.Parse(str, culture);

    /// <inheritdoc/>
    [Pure]
    protected override double ToRaw(Elo svo) => (double)svo;
}
