@NamespaceDeclaration

[global::System.ComponentModel.TypeConverter(typeof(@Svo.TypeConverter))]
readonly partial struct @Svo
{
    /// <summary>An singleton instance that deals with the @FullName specific behavior.</summary>
    [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
    private static readonly @Behavior behavior = new();

    /// <summary>Initializes a new instance of the <see cref="@Svo" /> struct.</summary>
    private @Svo(@Raw value) { m_Value = value; }

    /// <summary>The inner value of the Single Value Object.</summary>
    [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
    private readonly @Raw m_Value;

#if !StringBased // exec
    /// <summary>Casts the Single Value Object to a <see cref="@Raw" />.</summary>
    public static explicit operator @Raw(@Svo id) => id.m_Value;
#endif // exec

    private sealed class TypeConverter(global::System.Type underlying) : global::Qowaiv.Customization.CustomBehaviorTypeConverter<@Svo, @Raw, @Behavior>
    {
        /// <summary>Converts from the raw/underlying type.</summary>
        [global::System.Diagnostics.Contracts.Pure]
        protected override @Svo FromRaw(@Raw raw) => new(raw);

        /// <summary>Converts to the raw/underlying type.</summary>
        [global::System.Diagnostics.Contracts.Pure]
        protected override @Raw ToRaw(@Svo svo) => svo.m_Value;
    }
}
