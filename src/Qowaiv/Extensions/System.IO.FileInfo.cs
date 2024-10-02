using Qowaiv.IO;

namespace System.IO;

/// <summary>Extensions on <see cref="FileInfo" />.</summary>
public static class QowaivFileInfoExtensions
{
    /// <summary>Gets the size of the current file.</summary>
    [Pure]
    public static StreamSize GetStreamSize(this FileInfo fileInfo)
        => StreamSize.FromFileInfo(fileInfo);
}
