using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a Gender.</summary>
    public class GenderTypeConverter : SvoTypeConverter<Gender>
    {
        /// <inheritdoc/>
        [Pure]
        protected override Gender FromString(string str, CultureInfo culture) => Gender.Parse(str, culture);
    }
}
