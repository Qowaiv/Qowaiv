public partial struct @TSvo : IEquatable<@TSvo>
#if NET8_0_OR_GREATER
    , IEqualityOperators<@TSvo, @TSvo, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is @TSvo other && Equals(other);

#if !NotEqualsSvo // exec
    /// <summary>Returns true if this instance and the other @FullName are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="@TSvo" /> to compare with.</param>
    [Pure]
    public bool Equals(@TSvo other) => m_Value == other.m_Value;

#endif // exec
#if !NotEqualsSvoAndGetHashCode // exec
    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Value);

#endif // exec
    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(@TSvo left, @TSvo right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(@TSvo left, @TSvo right) => !(left == right);
}
