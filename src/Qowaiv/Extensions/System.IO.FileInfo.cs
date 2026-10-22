using Qowaiv.IO;

namespace System.IO;

/// <summary>Extensions on <see cref="FileInfo" />.</summary>
public static class QowaivFileInfoExtensions
{
    extension(FileInfo fileInfo)
    {
        /// <summary>Gets the size of the current file.</summary>
        [Pure]
        public StreamSize GetStreamSize()
            => StreamSize.FromFileInfo(fileInfo);
    }
}
