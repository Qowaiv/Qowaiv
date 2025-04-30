namespace @Namespace;

[global::System.ComponentModel.TypeConverter(typeof(@Behavior))]
readonly partial struct @Svo
{
    /// <summary>An singleton instance that deals with the @FullName specific behavior.</summary>
    [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
    private static readonly @Behavior behavior = new();

    /// <summary>Initializes a new instance of the <see cref="@Svo" /> struct.</summary>
    private @Svo(@Value value) { m_Value = value; }

    /// <summary>The inner value of the Single Value Object.</summary>
    [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
    private readonly @Value m_Value;
}
