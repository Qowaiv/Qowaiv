using System;
using System.IO;

namespace Qowaiv.UnitTests.IO
{
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
    public class TemporaryDirectory : IDisposable
    {
        /// <summary>Creates a temporary directory.</summary>
        public TemporaryDirectory()
        {
            Root = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Uuid.NewUuid().ToString()));
            Root.Create();
        }

        /// <summary>Gets the full name of the directory.</summary>
        public string FullName { get { return Root.FullName; } }

        /// <summary>Casts the temporary directory to a <see cref="DirectoryInfo"/>.</summary>
        public static implicit operator DirectoryInfo(TemporaryDirectory dir) { return dir.Root; }

        /// <summary>Represents the temporary directory as <see cref="string"/>.</summary>
        public override string ToString() { return Root.ToString(); }

        /// <summary>The underlying <see cref="DirectoryInfo"/>.</summary>
        protected DirectoryInfo Root { get; private set; }

        /// <summary>Creates a file in the temporary directory.</summary>
        public FileInfo CreateFile(string fileName)
        {
            return new FileInfo(Path.Combine(FullName, fileName));
        }

        #region IDisposable

        // To detect redundant calls
        private bool isDisposed;

        protected virtual void Dispose(bool disposing)
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

        /// <summary>Disposes the temporary directory by deleting it and its content.</summary>
        public void Dispose()
        {
            Dispose(true);
        }
        
        #endregion
    }
}
