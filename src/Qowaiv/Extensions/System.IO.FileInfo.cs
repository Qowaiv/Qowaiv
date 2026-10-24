using Qowaiv.IO;
using System.IO;

namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="FileInfo" />.</summary>
public static class QowaivFileInfo
{
    extension(FileInfo fileInfo)
    {
        /// <summary>Gets the size of the current file.</summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StreamSize GetStreamSize()
            => StreamSize.FromFileInfo(fileInfo);
    }
}
