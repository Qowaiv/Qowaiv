using System;

namespace Qowaiv
{
    /// <summary>Extensions on <see cref="Guid"/> and <see cref="Uuid"/>.</summary>
    public static class UuidExtensions
    {
        /// <summary>Gets the version of the <see cref="Guid"/>.</summary>
        public static UuidVersion GetVersion(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            var version = bytes[7] >> 4;
            return (UuidVersion)version;
        }

        internal static void SetVersion(byte[] uuid, UuidVersion version)
        {
            uuid[7] &= 0x0F;
            uuid[7] |= unchecked((byte)((int)version << 4));
        }
    }
}
