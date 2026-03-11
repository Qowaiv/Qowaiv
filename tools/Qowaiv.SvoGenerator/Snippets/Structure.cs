#nullable enable

namespace @Namespace;

#if !NotStructure // exec
public partial struct @TSvo
{
#endif // exec
#if !NotField // exec
    private @TSvo(@type_n value) => m_Value = value;

    /// <summary>The inner value of the @FullName.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly @type_n m_Value;
#endif // exec
#if !NotIsEmptyOrUnknown // exec

    /// <summary>False if the @FullName is empty or unknown, otherwise true.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsKnown => m_Value != default && m_Value != Unknown.m_Value;
#endif // exec
#if !NotIsUnknown // exec

    /// <summary>Returns true if the @FullName is unknown, otherwise false.</summary>
    [Pure]
    public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif // exec
#if !NotIsEmptyOrUnknown // exec

    /// <summary>Returns true if the @FullName is empty or unknown, otherwise false.</summary>
    [Pure]
    public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif // exec
#if !NotStructure // exec
}
#endif // exec

#if !NotIsEmpty // exec
public partial struct @TSvo : IEmpty<@TSvo>
{
    /// <summary>Represents an empty/not set @FullName.</summary>
    public static @TSvo Empty => default;

    /// <summary>False if the @FullName is empty, otherwise true.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool HasValue => m_Value != default;

    /// <summary>Returns true if the @FullName is empty, otherwise false.</summary>
    [Pure]
    public bool IsEmpty() => !HasValue;
}
#endif // exec
