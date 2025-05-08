partial struct @Svo
{
    /// <summary>Gets a <see cref="byte" /> array that represents the <see cref="@Svo" />.</summary>
    [global::System.Diagnostics.Contracts.Pure]
    public byte[] ToByteArray() => HasValue ? behavior.ToByteArray(m_Value) : [];

    /// <summary>Creates the <see cref="@Svo" /> for the <see cref="global::System.Byte" /> array.</summary>
    /// <param name="bytes">
    /// The <see cref="global::System.Byte" /> array that represents the underlying value.
    /// </param>
    [global::System.Diagnostics.Contracts.Pure]
    public static @Svo FromBytes(global::System.Byte[]? bytes)
        => bytes is not { Length: > 0 }
        ? Empty
        : new(behavior.FromBytes(bytes));

    /// <summary>Creates the next instance of the <see cref="Svo" />.</summary>
    [global::System.Diagnostics.Contracts.Pure]
    public static @Svo Next() => new(behavior.NextId());
}
