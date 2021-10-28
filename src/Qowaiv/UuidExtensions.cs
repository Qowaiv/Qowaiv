using System;
using System.Diagnostics.Contracts;

namespace Qowaiv
{
    /// <summary>Extensions on <see cref="Guid"/> and <see cref="Uuid"/>.</summary>
    public static class UuidExtensions
    {
        /// <summary>Gets the version of the <see cref="Guid"/>.</summary>
        [Pure]
        public static UuidVersion GetVersion(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            var version = bytes[Uuid.IndexOfVersion] >> 4;
            return (UuidVersion)version;
        }

        internal static void SetVersion(byte[] uuid, UuidVersion version)
        {
            uuid[Uuid.IndexOfVersion] &= 0x0F;
            uuid[Uuid.IndexOfVersion] |= unchecked((byte)((int)version << 4));
        }
    }
}
