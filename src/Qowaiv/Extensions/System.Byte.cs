using Qowaiv.IO;

namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="byte" />.</summary>
public static class QowaivBytes
{
    extension(byte[] bytes)
    {
        /// <summary>Gets the size of the byte array.</summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StreamSize GetStreamSize()
            => StreamSize.FromByteArray(bytes);
    }
}
