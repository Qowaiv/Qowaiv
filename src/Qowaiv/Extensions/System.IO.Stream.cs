using Qowaiv.IO;

namespace System.IO;

/// <summary>Extensions on <see cref="Stream" />.</summary>
public static class QowaivStreamExtensions
{
    extension(Stream stream)
    {
        /// <summary>Gets the stream size of the <see cref="Stream" />.</summary>
        [Pure]
        public StreamSize GetStreamSize()
            => StreamSize.FromStream(stream);
    }
}
