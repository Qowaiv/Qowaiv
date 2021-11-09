using Qowaiv.IO;

namespace System;

/// <summary>Extensions on <see cref="byte"/>.</summary>
public static class QowaivByteExtensions
{
    /// <summary>Gets the size of the byte array.</summary>
    [Pure]
    public static StreamSize GetStreamSize(this byte[] bytes)
        => StreamSize.FromByteArray(bytes);
}
