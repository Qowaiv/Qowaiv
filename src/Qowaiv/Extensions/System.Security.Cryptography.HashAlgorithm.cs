using Qowaiv;
using Qowaiv.Security.Cryptography;

namespace System.Security.Cryptography;

/// <summary>Extensions on <see cref="HashAlgorithm" />.</summary>
public static class QowaivHashAlgorithmExtensions
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
