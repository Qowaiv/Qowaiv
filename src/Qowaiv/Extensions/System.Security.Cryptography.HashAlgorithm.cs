using Qowaiv;
using Qowaiv.Security.Cryptography;
using System.Security.Cryptography;

namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="HashAlgorithm" />.</summary>
public static class QowaivHashAlgorithm
{
    extension(HashAlgorithm algorithm)
    {
        /// <summary>Computes the hash value for the specified byte array.</summary>
        /// <param name="buffer">
        /// The input to compute the cryptographic seed for.
        /// </param>
        [Pure]
        public CryptographicSeed ComputeCryptographicSeed(byte[]? buffer)
            => buffer is { }
            ? CryptographicSeed.Create(Guard.NotNull(algorithm).ComputeHash(buffer))
            : CryptographicSeed.Empty;
    }
}
