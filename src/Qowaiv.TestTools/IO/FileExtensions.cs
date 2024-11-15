using System.IO;

namespace Qowaiv.TestTools.IO;

/// <summary>Extensions on <see cref="FileInfo"/>.</summary>
public static class FileExtensions
{
    /// <summary>Creates a lock on a <see cref="FileInfo"/>.</summary>
    [Impure]
    public static FileLock Lock(this FileInfo file) => new(file);
}
