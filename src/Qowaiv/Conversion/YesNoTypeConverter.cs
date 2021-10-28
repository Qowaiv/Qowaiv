using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a Yes-no.</summary>
    public class YesNoTypeConverter : SvoTypeConverter<YesNo>
    {
        /// <inheritdoc/>
        [Pure]
        protected override YesNo FromString(string str, CultureInfo culture) => YesNo.Parse(str, culture);
    }
}
