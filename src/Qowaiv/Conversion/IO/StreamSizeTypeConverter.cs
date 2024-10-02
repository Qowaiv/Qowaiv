using Qowaiv.IO;

namespace Qowaiv.Conversion.IO;

/// <summary>Provides a conversion for a stream size.</summary>
[Inheritable]
public class StreamSizeTypeConverter : NumericTypeConverter<StreamSize, long>
{
    /// <inheritdoc/>
    [Pure]
    protected override StreamSize FromRaw(long raw) => raw;

    /// <inheritdoc/>
    [Pure]
    protected override StreamSize FromString(string? str, CultureInfo? culture) => StreamSize.Parse(str, culture);

    /// <inheritdoc/>
    [Pure]
    protected override long ToRaw(StreamSize svo) => (long)svo;
}
