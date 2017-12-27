using System;

namespace Qowaiv
{
    /// <summary>The version of the <see cref="Uuid"/> generation.</summary>
    public enum UuidVersion
    {
        /// <summary>Empty (the version <see cref="Uuid.Empty"/>.</summary>
        Empty = 0,
        /// <summary>Generated <see cref="DateTime"/> and MAC address.</summary>
        Version1 = 1,
        /// <summary>Generated <see cref="DateTime"/> and MAC address, DCE security version.</summary>
        Version2 = 2,
        /// <summary>Generated using <see cref="System.Security.Cryptography.MD5"/> hashing.</summary>
        MD5 = 3,
        /// <summary>Randomly generated <see cref="Uuid"/>.</summary>
        Random = 4,
        /// <summary>Generated using <see cref="System.Security.Cryptography.SHA1"/> hashing.</summary>
        SHA1 = 5,
    }
}
