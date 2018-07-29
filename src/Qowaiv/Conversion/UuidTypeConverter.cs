using System;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a GUID.</summary>
    public class UuidTypeConverter : SvoTypeConverter<Uuid, Guid>
    {
        /// <inheritdoc />
        protected override Uuid FromString(string str, CultureInfo culture) => Uuid.Parse(str);

        /// <inheritdoc />
        protected override Uuid FromRaw(Guid raw) => raw;

        /// <inheritdoc />
        protected override Guid ToRaw(Uuid svo) => svo;
    }
}
