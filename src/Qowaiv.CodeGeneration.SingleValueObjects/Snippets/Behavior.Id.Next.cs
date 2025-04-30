partial struct @Svo
{
    /// <summary>Creates the next instance of <see cref="Svo"/>.</summary>
    [global::System.Diagnostics.Contracts.Pure]
    public static @Svo Next() => new(behavior.NextId());
}
