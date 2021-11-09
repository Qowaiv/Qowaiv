using Qowaiv.Sql;

namespace Qowaiv.Conversion.Sql;

/// <summary>Provides a conversion for a time stamp.</summary>
[CLSCompliant(false /* based on the non compliant UInt64. */)]
public class TimestampTypeConverter : NumericTypeConverter<Timestamp, ulong>
{
    /// <inheritdoc/>
    [Pure]
    protected override Timestamp FromRaw(ulong raw) => raw;

    /// <inheritdoc/>
    [Pure]
    protected override Timestamp FromString(string str, CultureInfo culture) => Timestamp.Parse(str, culture);

    /// <inheritdoc/>
    [Pure]
    protected override ulong ToRaw(Timestamp svo) => (ulong)svo;
}
