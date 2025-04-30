partial struct @Svo : global::System.IComparable, global::System.IComparable<@Svo>
{
    /// <inheritdoc />
    [global::System.Diagnostics.Contracts.Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) return +1;
        else if (obj is @Svo other) return CompareTo(other);
        else throw new global::System.ArgumentException($"Argument must be {GetType().Name}.", nameof(obj));
    }

    /// <inheritdoc />
    [global::System.Diagnostics.Contracts.Pure]
    public int CompareTo(@Svo other) => behavior.Compare(m_Value, other.m_Value);
}
