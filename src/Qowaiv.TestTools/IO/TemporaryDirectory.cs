using System.IO;

namespace Qowaiv.TestTools.IO;

/// <summary>Represents a directory that lives during the lifetime of its scope.</summary>
/// <remarks>
/// Should always been used with a using statement.
/// 
/// <code>
/// using(var directory = new TemporaryDirectory()
/// {
///     // Do stuff.
/// }
/// </code>
/// </remarks>
public sealed class TemporaryDirectory : IDisposable
{
    /// <summary>Creates a temporary directory.</summary>
    public TemporaryDirectory()
    {
        Root = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Uuid.NewUuid().ToString()));
        Root.Create();
    }

    /// <summary>Gets the full name of the directory.</summary>
    public string FullName => Root.FullName;

    /// <summary>Casts the temporary directory to a <see cref="DirectoryInfo"/>.</summary>
    public static implicit operator DirectoryInfo?(TemporaryDirectory dir) => dir?.Root;

    /// <summary>Represents the temporary directory as <see cref="string"/>.</summary>
    [Pure]
    public override string ToString() => Root.ToString();

    /// <summary>The underlying <see cref="DirectoryInfo"/>.</summary>
    private readonly DirectoryInfo Root;

    /// <summary>Creates a file in the temporary directory.</summary>
    [Pure]
    public FileInfo CreateFile(string fileName) => new(Path.Combine(FullName, fileName));

    /// <summary>Disposes the temporary directory by deleting it and its content.</summary>
    public void Dispose() => Dispose(true);

    private void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                Root.Delete(true);
            }
            isDisposed = true;
        }
    }

    private bool isDisposed;
}
