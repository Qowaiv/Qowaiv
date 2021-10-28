using Qowaiv.Web;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Conversion.Web
{
    /// <summary>Provides a conversion for an Internet media type.</summary>
    public class InternetMediaTypeTypeConverter : SvoTypeConverter<InternetMediaType>
    {
        /// <inheritdoc/>
        [Pure]
        protected override InternetMediaType FromString(string str, CultureInfo culture) => InternetMediaType.Parse(str);
    }
}
