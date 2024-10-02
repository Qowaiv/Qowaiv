using Qowaiv.IO;

namespace System.IO;

/// <summary>Extensions on <see cref="Stream" />.</summary>
public static class QowaivStreamExtensions
{
    /// <summary>Gets the stream size of the <see cref="Stream" />.</summary>
    [Pure]
    public static StreamSize GetStreamSize(this Stream stream)
        => StreamSize.FromStream(stream);
}
