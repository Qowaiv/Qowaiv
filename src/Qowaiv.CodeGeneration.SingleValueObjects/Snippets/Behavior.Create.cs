#if !NotStringBased // exec
partial struct @Svo
{
    /// <summary>Creates a new instance of the <see cref="@Svo" /> struct.</summary>
    /// <param name="value">
    /// The desired underlying value.
    /// </param>
    [global::System.Diagnostics.Contracts.Pure]
    public static @Svo Create(@Raw value)
        => TryCreate(value)
        ?? throw new global::System.InvalidCastException($"Could not cast {typeof(@Raw)} to {typeof(@Svo)}");

    /// <summary>Creates a new instance of the <see cref="@Svo" /> struct.</summary>
    /// <param name="value">
    /// The desired underlying value.
    /// </param>
    [global::System.Diagnostics.Contracts.Pure]
    public static @Svo? TryCreate(@Raw value)
        => behavior.TryTransform(@value, out var tranformed)
        ? new @Svo(tranformed)
        : default;
}
#endif // exec
