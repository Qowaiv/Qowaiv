using Qowaiv;
using Qowaiv.IO;

namespace System.IO;

/// <summary>Extensions on <see cref="DirectoryInfo" />.</summary>
public static class QowaivDirectoryInfoExtensions
{
    /// <summary>Gets the stream size of the current directory.</summary>
    [Pure]
    public static StreamSize GetStreamSize(this DirectoryInfo directoryInfo)
        => Guard.NotNull(directoryInfo)
        .EnumerateFiles("*", SearchOption.AllDirectories)
        .Sum(file => file.Length);
}
