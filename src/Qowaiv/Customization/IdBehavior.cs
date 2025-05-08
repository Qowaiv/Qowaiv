namespace Qowaiv.Customization;

/// <summary>Handles the behavior of a custom identifier.</summary>
public abstract class IdBehavior<TValue> : CustomBehavior<TValue>
    where TValue : IEquatable<TValue>
{
    /// <summary>Returns a <see cref="byte" /> that represents the underlying value of the identifier.</summary>
    [Pure]
    public abstract byte[] ToByteArray(TValue id);

    /// <summary>Returns the underlying value of the identifier represented by a <see cref="byte" /> array.</summary>
    [Pure]
    public abstract TValue FromBytes(byte[] bytes);

    /// <summary>Creates a new (random) underlying value.</summary>
    [Pure]
    public virtual TValue NextId() => throw new NotSupportedException();
}
