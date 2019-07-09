using Qowaiv.Web;
using System.Globalization;

namespace Qowaiv.Conversion.Web
{
    /// <summary>Provides a conversion for an Internet media type.</summary>
    public class InternetMediaTypeTypeConverter : SvoTypeConverter<InternetMediaType>
    {
        /// <inheritdoc/>
        protected override InternetMediaType FromString(string str, CultureInfo culture) => InternetMediaType.Parse(str);
    }
}
