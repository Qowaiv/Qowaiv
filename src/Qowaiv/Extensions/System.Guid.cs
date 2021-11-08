using Qowaiv;
using Qowaiv.IO;
using System.Diagnostics.Contracts;

namespace System
{
    /// <summary>Extensions on <see cref="Guid"/>.</summary>
    public static class QowaivGuidExtensions
    {
        /// <summary>Gets the version of the <see cref="Guid"/>.</summary>
        [Pure]
        public static UuidVersion GetVersion(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            var version = bytes[Uuid.IndexOfVersion] >> 4;
            return (UuidVersion)version;
        }
    }
}
