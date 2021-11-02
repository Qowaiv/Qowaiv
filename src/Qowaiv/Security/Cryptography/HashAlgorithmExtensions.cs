using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace Qowaiv.Security.Cryptography
{
    public static class HashAlgorithmExtensions
    {
        [Pure]
        public static Secret ComputeSecret(this HashAlgorithm algorithm, byte[] buffer)
        {
            Guard.NotNull(algorithm, nameof(algorithm));
            var hash = algorithm.ComputeHash(buffer);
            throw new NotSupportedException();
        }
    }
}
