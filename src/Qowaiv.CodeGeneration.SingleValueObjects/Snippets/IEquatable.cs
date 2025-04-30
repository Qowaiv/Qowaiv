partial struct @Svo : global::System.IEquatable<@Svo>
#if NET8_0_OR_GREATER
    , global::System.Numerics.IEqualityOperators<@Svo, @Svo, bool>
#endif
{
    /// <inheritdoc />
    [global::System.Diagnostics.Contracts.Pure]
    public override bool Equals([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is @Svo other && Equals(other);

    /// <inheritdoc />
    [global::System.Diagnostics.Contracts.Pure]
    public bool Equals(@Svo other) => m_Value == other.m_Value;

    /// <inheritdoc />
    [global::System.Diagnostics.Contracts.Pure]
    public override int GetHashCode() => global::Qowaiv.Hashing.Hash.Code(m_Value);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(@Svo left, @Svo right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(@Svo left, @Svo right) => !(left == right);
}
