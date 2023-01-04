using Qowaiv;
using Qowaiv.Security.Cryptography;

namespace System.Security.Cryptography;

/// <summary>Extensions on <see cref="HashAlgorithm"/>.</summary>
public static class QowaivHashAlgorithmExtensions
{
    /// <summary>Computes the hash value for the specified byte array.</summary>
    /// <param name="algorithm">
    /// The algorithm to use.
    /// </param>
    /// <param name="buffer">
    /// The input to compute the cryptographic seed for.
    /// </param>
    [Pure]
    public static CryptographicSeed ComputeCryptographicSeed(this HashAlgorithm algorithm, byte[] buffer)
        => buffer is { }
        ? CryptographicSeed.Create(Guard.NotNull(algorithm, nameof(algorithm)).ComputeHash(buffer))
        : CryptographicSeed.Empty;
}
