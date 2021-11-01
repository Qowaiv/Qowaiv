using Qowaiv.Diagnostics;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Qowaiv.Security.Cryptography
{
    /// <summary>Represents a secret.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public readonly struct Secret : IEquatable<Secret>
    {
        /// <summary>Represents an empty/not set secret.</summary>
        public static readonly Secret Empty;

        /// <summary>Gets the length of the secret.</summary>
        public int Length => bytes is null ? 0 : bytes.Length;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly byte[] bytes;

        private Secret(byte[] bytes) => this.bytes = bytes;

        /// <summary>Gets the bytes of this secret.</summary>
        [Pure]
        public byte[] ToByteArray()
            => Length > 0
            ? bytes.ToArray()
            : Array.Empty<byte>();

        /// <summary>Gets an UTF8 string representing this secret.</summary>
        [Pure]
        public string UTF8() => Encoding.UTF8.GetString(bytes ?? Array.Empty<byte>());

        /// <summary>Gets an Base-64 string representing this secret.</summary>
        [Pure]
        public string Base64() => Text.Base64.ToString(bytes);

        /// <inheritdoc />
        [Pure]
        public override bool Equals(object obj) => obj is Secret other && Equals(other);
        
        /// <inheritdoc />
        [Pure]
        public bool Equals(Secret other)
        {
            if (Length == other.Length)
            {
                for (var i = 0; i < Length; i++)
                {
                    if (!bytes[i].Equals(other.bytes[i])) return false;
                }
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode()
        {
            var hash = 0;
            for (var i = 0; i < Length; i++)
            {
                hash ^= bytes[i] << ((i * 17) % 24);
            }
            return hash;
        }

        /// <inheritdoc />
        [Pure]
        public override string ToString() => Length == 0 ? string.Empty : "???";

        /// <summary>Returns a <see cref="string" /> that represents the current date span for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
            => Length == 0
            ? DebugDisplay.Empty
            : $"Base-64: {Base64()}, UTF8: {UTF8()}";

        /// <summary>Creates a secret from an UTF8 string.</summary >
        /// <param name="str" >
        /// A secret describing a cryptographic seed.
        /// </param >
        [Pure]
        public static Secret Parse(string str)
            => string.IsNullOrEmpty(str)
            ? Empty
            : new(Encoding.UTF8.GetBytes(str));

        /// <summary>Creates a secret from a byte[]. </summary >
        [Pure]
        public static Secret Create(byte[] val)
            => val == null || val.Length == 0
            ? Empty
            : new(val.ToArray());

        /// <summary>Creates a secret from an GUID.</summary>
        [Pure]
        public static Secret Create(Guid id) => new(id.ToByteArray());

        /// <summary>Creates a secret from an UUID.</summary>
        [Pure]
        public static Secret Create(Uuid id) => new(id.ToByteArray());

        /// <summary>Creates a secret from a JSON string node.</summary>
        [Pure]
        public static Secret FromJson(string json) => Parse(json);
    }
}
