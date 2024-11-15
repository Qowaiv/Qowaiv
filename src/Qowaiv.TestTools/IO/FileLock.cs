using System.IO;

namespace Qowaiv.TestTools.IO;

/// <summary>Creates a lock on a file for its existence.</summary>
[DebuggerDisplay("{Info}")]
public sealed class FileLock : IDisposable
{
    internal FileLock(FileInfo file)
    {
        Info = Guard.NotNull(file);
        Stream = new(file.FullName, FileMode.Open, FileAccess.Read, FileShare.None);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly FileInfo Info;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly FileStream Stream;

    /// <summary>Casts the temporary directory to a <see cref="FileInfo" />.</summary>
    [return: NotNullIfNotNull(nameof(@lock))]
    public static implicit operator FileInfo?(FileLock? @lock) => @lock?.Info;

    /// <inheritdoc />
    public void Dispose()
    {
        if (!Disposed)
        {
            Stream.Dispose();
            Disposed = true;
        }
    }

    private bool Disposed;
}
