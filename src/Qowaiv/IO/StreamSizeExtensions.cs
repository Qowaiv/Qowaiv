using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace Qowaiv.IO
{
    /// <summary>Contains extensions for stream size.</summary>
    public static class StreamSizeExtensions
    {
        /// <summary>Gets the size of the byte array.</summary>
        [Pure]
        public static StreamSize GetStreamSize(this byte[] bytes)
            => StreamSize.FromByteArray(bytes);

        /// <summary>Gets the size of the current file.</summary>
        [Pure]
        public static StreamSize GetStreamSize(this FileInfo fileInfo)
            => StreamSize.FromFileInfo(fileInfo);

        /// <summary>Gets the stream size of the current directory.</summary>
        [Pure]
        public static StreamSize GetStreamSize(this DirectoryInfo directoryInfo)
            => Guard.NotNull(directoryInfo, nameof(directoryInfo))
            .EnumerateFiles("*", SearchOption.AllDirectories)
            .Sum(file => file.Length);

        /// <summary>Gets the stream size of the current stream.</summary>
        [Pure]
        public static StreamSize GetStreamSize(this Stream stream)
            => StreamSize.FromStream(stream);

        /// <summary>Computes the sum of a sequence of stream sizes.</summary>
        [Pure]
        public static StreamSize Sum(this IEnumerable<StreamSize> streamSizes)
            => new(streamSizes.Sum(streamSize => (long)streamSize));

        /// <summary>Computes the average of a sequence of stream sizes.</summary>
        [Pure]
        public static StreamSize Average(this IEnumerable<StreamSize> streamSizes)
            => new((long)streamSizes.Average(streamSize => (long)streamSize));
    }
}
