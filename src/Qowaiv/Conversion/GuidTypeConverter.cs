using Qowaiv.Identifiers;
using System;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a GUID.</summary>
    public class GuidTypeConverter : SvoTypeConverter<Guid>
    {
        /// <inheritdoc />
        protected override Guid FromString(string str, CultureInfo culture)
        {
            if (GuidBehavior.Instance.TryParse(str, out var id))
            {
                return id is Guid guid ? guid : Guid.Empty;
            }
            throw new FormatException(QowaivMessages.FormatExceptionUuid);
        }
    }
}
