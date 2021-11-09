using Qowaiv.Mathematics;

namespace Qowaiv.Conversion.Mathematics;

/// <summary>Provides a conversion for an email address.</summary>
public class FractionTypeConverter : SvoTypeConverter<Fraction>
{
    /// <inheritdoc/>
    [Pure]
    protected override Fraction FromString(string str, CultureInfo culture) => Fraction.Parse(str, culture);
}
