using System.IO;
using Qowaiv.IO;

namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="Stream" />.</summary>
public static class QowaivStreamExtensions
{
    extension(Stream stream)
    {
        /// <summary>Gets the stream size of the <see cref="Stream" />.</summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StreamSize GetStreamSize()
            => StreamSize.FromStream(stream);
    }
}
