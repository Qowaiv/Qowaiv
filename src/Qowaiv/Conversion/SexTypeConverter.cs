using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a Sex.</summary>
    public class SexTypeConverter : SvoTypeConverter<Sex>
    {
        /// <inheritdoc/>
        [Pure]
        protected override Sex FromString(string str, CultureInfo culture) => Sex.Parse(str, culture);
    }
}
