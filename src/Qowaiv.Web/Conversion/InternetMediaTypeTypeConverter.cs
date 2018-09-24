using Qowaiv.Conversion;
using System.Globalization;

namespace Qowaiv.Web.Conversion
{
    /// <summary>Provides a conversion for an Internet media type.</summary>
    public class InternetMediaTypeTypeConverter : SvoTypeConverter<InternetMediaType>
    {
        /// <inheritdoc/>
        protected override InternetMediaType FromString(string str, CultureInfo culture) => InternetMediaType.Parse(str);
    }
}
