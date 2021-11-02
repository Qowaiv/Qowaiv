using Qowaiv.Diagnostics;
using Qowaiv.Hashing;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Qowaiv.Security.Cryptography
{
    /// <summary>Represents a secret.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public readonly struct Secret : IEquatable<Secret>
    {
        /// <summary>Represents an empty/not set secret.</summary>
        public static readonly Secret Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string m_Value;

        private Secret(string value) => m_Value = value;

        /// <summary>Returns true if the secret is empty/not set.</summary>
        [Pure]
        public bool IsEmpty() => m_Value is null;

        /// <summary>Gets a string representing this secret.</summary>
        [Pure]
        public string Value() => m_Value ?? string.Empty;

        /// <inheritdoc />
        [Pure]
        public override bool Equals(object obj) => obj is Secret other && Equals(other);

        /// <inheritdoc />
        [Pure]
        public bool Equals(Secret other) => m_Value == other.m_Value;

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => Hash.NotSupportedBy<Secret>();

        /// <inheritdoc />
        [Pure]
        public override string ToString() => m_Value is null ? string.Empty : "???";

        /// <summary>Returns a <see cref="string" /> that represents the current date span for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => m_Value is null ? DebugDisplay.Empty : Value();

        /// <summary>Creates a secret from an UTF8 string.</summary >
        /// <param name="str" >
        /// A secret describing a cryptographic seed.
        /// </param >
        [Pure]
        public static Secret Parse(string str)
            => string.IsNullOrEmpty(str)
            ? Empty
            : new(str);
     
        /// <summary>Creates a secret from a JSON string node.</summary>
        [Pure]
        public static Secret FromJson(string json) => Parse(json);
    }
}
