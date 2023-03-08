namespace Qowaiv.Hashing;

/// <summary>Helper struct for getting randomized hashcodes.</summary>
/// <remarks>
/// Inspired by https://rehansaeed.com/gethashcode-made-easy.
/// </remarks>
public readonly struct Hash : IEquatable<Hash>
{
    /// <summary>Randomizer hash based on <see cref="string.GetHashCode()"/>.</summary>
    private static int Randomized = "QOWAIV".GetHashCode();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly int Value;

    /// <summary>Initializes a new instance of the <see cref="Hash"/> struct.</summary>
    private Hash(int val) => Value = val;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Value.ToString();

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is Hash other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(Hash other) => Value == other.Value;

    /// <summary>Throws a <see cref="HashingNotSupported" />.</summary>
    [Pure]
    [EditorBrowsable(EditorBrowsableState.Never)]
#if NET5_0_OR_GREATER
    [DoesNotReturn]
#endif
    [WillBeSealed]
    public override int GetHashCode() => NotSupportedBy<Hash>();

    /// <summary>extends the hash with the added item.</summary>
    /// <typeparam name="T">
    /// The type of the item.
    /// </typeparam>
    /// <param name="item">
    /// The item.
    /// </param>
    /// <returns>The new hash.</returns>
    [Pure]
    public Hash And<T>(T item)
        => Equals(default(T), item)
        ? this
        : new(Combine(Value, HashCode(item)));

    /// <summary>Implicitly casts a <see cref="Hash"/> to an <see cref="int"/>.</summary>
    public static implicit operator int(Hash hash) => hash.Value;

    /// <summary>Gets a randomized hash for the item.</summary>
    [Pure]
    public static Hash Code<T>(T item)
        => Equals(default(T), item)
        ? default
        : new(Combine(Randomized, HashCode(item)));

    /// <summary>Indicates that hashing is not supported by the type.</summary>
    [Pure]
#if NET5_0_OR_GREATER
    [DoesNotReturn]
#endif
    public static int NotSupportedBy<T>()
        => throw new HashingNotSupported(string.Format(QowaivMessages.HashingNotSupported, typeof(T)));

    /// <summary>Fixes the base hash for the scope of the statement.</summary>
    /// <remarks>
    /// This should only be used in tests.
    /// </remarks>
    public static IDisposable WithoutRandomizer() => new Scope();

    /// <summary>Combines two hashes with `((h1 &lt;&lt; 5) + h1) ^ h2`.</summary>
    [Pure]
    private static int Combine(int h1, int h2) => unchecked(((h1 << 5) + h1) ^ h2);

    /// <summary>Gets a hash, based on its type.</summary>
    [Pure]
    private static int HashCode<T>(T obj)
        => obj switch
        {
            null => 0,
            int int32 => int32,
            string str => HashCode(str),
            IEnumerable enumerable => HashCodes(enumerable),
            _ => obj.GetHashCode(),
        };

    /// <summary>Gets a deterministic hash of a string.</summary>
    [Pure]
    private static int HashCode(string str)
    {
        unchecked
        {
            int hash1 = (5381 << 16) + 5381;
            int hash2 = hash1;

            for (int i = 0; i < str.Length; i += 2)
            {
                hash1 = Combine(hash1, str[i]);
                if (i != str.Length - 1)
                {
                    hash2 = Combine(hash2, str[i + 1]);
                }
            }
            return hash1 + (hash2 * 1566083941);
        }
    }

    /// <summary>Gets a hash for an enumerable.</summary>
    [Pure]
    private static int HashCodes(IEnumerable items)
    {
        int hash = 0;
        var enumerator = items.GetEnumerator();
        if (enumerator.MoveNext())
        {
            var updated = Combine(hash, HashCode(enumerator.Current));
            while (enumerator.MoveNext())
            {
                updated = Combine(updated, HashCode(enumerator.Current));
            }
            return updated;
        }
        else return 17;
    }

#pragma warning disable S3010 // Static fields should not be updated in constructors
#pragma warning disable S2696 // Instance members should not write to "static" fields
    /// <remarks>This class deals with temporary changing the state of a private static field.</remarks>
    private sealed class Scope : IDisposable
    {
        private readonly int Current;

        public Scope()
        {
            Current = Randomized;
            Randomized = 20170611;
        }

        /// <inheritdoc />
        public void Dispose() => Randomized = Current;
    }
}
