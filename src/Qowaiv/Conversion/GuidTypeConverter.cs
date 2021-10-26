using Qowaiv.Identifiers;
using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a GUID.</summary>
    [Obsolete("Will be dropped when the next major version is released.")]
    public class GuidTypeConverter : SvoTypeConverter<Guid>
    {
        /// <inheritdoc />
        [Pure]
        protected override Guid FromString(string str, CultureInfo culture)
        {
            if (GuidBehavior.Instance.TryParse(str, out var id))
            {
                return id is Guid guid ? guid : Guid.Empty;
            }
            else throw new FormatException(QowaivMessages.FormatExceptionUuid);
        }
    }
}
