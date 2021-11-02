using System;
using System.Diagnostics.Contracts;

namespace Qowaiv.Hashing
{
    /// <summary>Helper class for getting randomized hashcodes.</summary>
    public static class Hash
    {
        /// <summary>Gets a randomized hashcode for a <see cref="string"/>.</summary>
        [Pure]
        public static int Code(string str)
        {
            str ??= string.Empty;
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1) break;
                    else  hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }
                return Code(hash1 + (hash2 * 1566083941));
            }
        }

        /// <summary>Gets a randomized hashcode for an <see cref="int"/>.</summary>
        [Pure]
        public static int Code(int value) => Randomizer ^ value;
        
        /// <summary>Gets a randomized hashcode for a struct.</summary>
        [Pure]
        public static int Code<T>(T value) where T : struct
            => Randomizer ^ value.GetHashCode();

        /// <summary>Randomizer hash based on <see cref="string.GetHashCode()"/>.</summary>
        private static int Randomizer = "Qowaiv".GetHashCode();

        /// <summary>Indicates that hashing is not supported by the type.</summary>
        [Pure]
        public static int NotSupportedBy<T>() 
            => throw new HashingNotSupported(string.Format(QowaivMessages.HashingNotSupported, typeof(T)));

        /// <summary>Fixes the base hash for the scope of the statement.</summary>
        public static IDisposable WithFixedRandomizer() => new Scope();

#pragma warning disable S3010 // Static fields should not be updated in constructors
#pragma warning disable S2696 // Instance members should not write to "static" fields
        /// <remarks>This class deals with temporary changing the state of a private static field.</remarks>
        private sealed class Scope : IDisposable
        {
            private readonly int Current;

            public Scope()
            {
                Current = Randomizer;
                Randomizer = 20170611;
            }

            public void Dispose() => Randomizer = Current;
        }
    }
}
