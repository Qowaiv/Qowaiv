using Qowaiv.IO;

namespace System;

/// <summary>Extensions on <see cref="byte" />.</summary>
public static class QowaivByteExtensions
{
    extension(byte[] bytes)
    {
        /// <summary>Gets the size of the byte array.</summary>
        [Pure]
        public StreamSize GetStreamSize()
            => StreamSize.FromByteArray(bytes);
    }
}
