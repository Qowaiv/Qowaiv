using Qowaiv.IO;
using System.IO;

namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="DirectoryInfo" />.</summary>
public static class QowaivDirectoryInfo
{
    extension(DirectoryInfo directoryInfo)
    {
        /// <summary>Gets the stream size of the current directory.</summary>
        [Pure]
        public StreamSize GetStreamSize()
            => Guard.NotNull(directoryInfo)
            .EnumerateFiles("*", SearchOption.AllDirectories)
            .Sum(file => file.Length);
    }
}
