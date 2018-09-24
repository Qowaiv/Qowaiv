using Qowaiv.IO;
using System.Globalization;

namespace Qowaiv.Conversion.IO
{
    /// <summary>Provides a conversion for a stream size.</summary>
    public class StreamSizeTypeConverter : NumericTypeConverter<StreamSize, long>
    {
        /// <inheritdoc/>
        protected override StreamSize FromRaw(long raw) => raw;

        /// <inheritdoc/>
        protected override StreamSize FromString(string str, CultureInfo culture) => StreamSize.Parse(str, culture);

        /// <inheritdoc/>
        protected override long ToRaw(StreamSize svo) => (long)svo;
    }
}
