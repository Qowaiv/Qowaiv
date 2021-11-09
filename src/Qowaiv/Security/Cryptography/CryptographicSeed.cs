﻿using Qowaiv.Conversion.Security.Cryptography;
using Qowaiv.Diagnostics;
using Qowaiv.Hashing;
using Qowaiv.Text;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Qowaiv.Security.Cryptography
{
    /// <summary>Represents a cryptographic seed.</summary>
    [TypeConverter(typeof(CryptographicSeedTypeConverter))]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public readonly struct CryptographicSeed : IEquatable<CryptographicSeed>
    {
        /// <summary>Represents an empty/not set cryptographic seed.</summary>
        public static readonly CryptographicSeed Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly byte[] m_Value;

        private CryptographicSeed(byte[] value) => m_Value = value;

        /// <summary>Returns true if the cryptographic seed is empty/not set.</summary>
        [Pure]
        public bool IsEmpty() => m_Value is null;

        /// <summary>Gets a string representing this cryptographic seed.</summary>
        [Pure]
        public string Value() => Base64.ToString(m_Value);

        /// <summary>Returns a byte array that contains the value of this instance.</summary>
        [Pure]
        public byte[] ToByteArray()
        {
            if (m_Value is null || m_Value.Length == 0) return Array.Empty<byte>();
            else
            {
                var clone = new byte[m_Value.Length];
                Array.Copy(m_Value, clone, clone.Length);
                return clone;
            }
        }
       
        /// <inheritdoc />
        [Pure]
        public override bool Equals(object obj) => obj is CryptographicSeed other && Equals(other);

        /// <summary>Returns true if both are empty, otherwise false.</summary>
        /// <remarks>
        /// Secrets are supposed to be passed around, without knowing its content.
        /// </remarks>
        [Pure]
        public bool Equals(CryptographicSeed other) => IsEmpty() && other.IsEmpty();

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => Hash.NotSupportedBy<CryptographicSeed>();

        /// <summary>Represents the cryptographic seed as "*****".</summary>
        /// <remarks>
        /// To prevent unintended exposure. 
        /// </remarks>
        [Pure]
        public override string ToString() => m_Value is null ? string.Empty : "*****";

        /// <summary>Converts the cryptographic seed to a JSON null node.</summary>
        /// <remarks>
        /// To prevent unintended exposure. 
        /// </remarks>
        [Pure]
        public object ToJson() => null;

        /// <summary>Returns a <see cref="string" /> that represents the current date span for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => m_Value is null ? DebugDisplay.Empty : Value();

        /// <summary>Creates a cryptographic seed from an UTF8 string.</summary >
        /// <param name="str" >
        /// A cryptographic seed describing a cryptographic seed.
        /// </param >
        [Pure]
        public static CryptographicSeed Parse(string str)
            => TryParse(str, out var seed)
            ? seed
            : throw new FormatException(QowaivMessages.FormatExceptionCryptographicSeed);

        /// <summary>Converts the string to a cryptographic seed.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a cryptographic seed to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out CryptographicSeed result)
        {
            result = Empty;
            if (string.IsNullOrEmpty(s)) return true;
            else if (Base64.TryGetBytes(s, out byte[] bytes))
            {
                result = Create(bytes);
                return true;
            }
            else return false;
        }

        /// <summary>Creates a cryptographic seed from a byte[].</summary>
        /// <param name="val" >
        /// A byte array describing a cryptographic seed.
        /// </param >
        [Pure]
        public static CryptographicSeed Create(byte[] val)
            => val == null || val.Length == 0
            ? Empty
            : new CryptographicSeed(val.ToArray());

        /// <summary>Creates a cryptographic seed from a JSON string node.</summary>
        [Pure]
        public static CryptographicSeed FromJson(string json) => Parse(json);
    }
}
