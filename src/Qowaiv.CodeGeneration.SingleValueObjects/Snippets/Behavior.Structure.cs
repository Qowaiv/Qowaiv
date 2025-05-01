namespace @Namespace;

[global::System.ComponentModel.TypeConverter(typeof(global::@Namespace.@Svo.TypeConverter))]
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

    private sealed class TypeConverter : global::Qowaiv.Customization.CustomBehaviorTypeConverter<@Svo, @Value, @Behavior>
    {
        /// <summary>Converts from the raw/underlying type.</summary>
        [global::System.Diagnostics.Contracts.Pure]
        protected override @Svo FromRaw(@Value raw) => new(raw);

        /// <summary>Converts to the raw/underlying type.</summary>
        [global::System.Diagnostics.Contracts.Pure]
        protected override @Value ToRaw(@Svo svo) => svo.m_Value;
    }
}
