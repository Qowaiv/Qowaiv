using Qowaiv.Sql;
using System;
using System.Globalization;

namespace Qowaiv.Conversion.Sql
{
    /// <summary>Provides a conversion for a time stamp.</summary>
    [CLSCompliant(false /* based on the non compliant UInt64. */)]
    public class TimestampTypeConverter : NumericTypeConverter<Timestamp, ulong>
    {
        /// <inheritdoc/>
        protected override Timestamp FromRaw(ulong raw) => raw;

        /// <inheritdoc/>
        protected override Timestamp FromString(string str, CultureInfo culture) => Timestamp.Parse(str, culture);

        /// <inheritdoc/>
        protected override ulong ToRaw(Timestamp svo) => (ulong)svo;
    }
}
