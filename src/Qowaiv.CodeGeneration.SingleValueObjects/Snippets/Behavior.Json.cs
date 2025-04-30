#if NET6_0_OR_GREATER
    [global::System.Text.Json.Serialization.JsonConverter(typeof(SvoJsonConverter))]
#endif
partial struct @Svo
{
    /// <summary>Creates the @FullName from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized @FullName.
    /// </returns>
    [global::System.Diagnostics.Contracts.Pure]
    public static @Svo FromJson(string? json) => Parse(json, global::System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>Serializes the @FullName to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [global::System.Diagnostics.Contracts.Pure]
    public object? ToJson() => behavior.ToJson(m_Value);

#if NET6_0_OR_GREATER
    private sealed class SvoJsonConverter : global::Qowaiv.Json.SvoJsonConverter<@Svo>
    {
        /// <inheritdoc />
        [global::System.Diagnostics.Contracts.Pure]
        protected override @Svo FromJson(string? json) => @Svo.FromJson(json);

        /// <inheritdoc />
        [global::System.Diagnostics.Contracts.Pure]
        protected override object? ToJson(@Svo svo) => svo.ToJson();
    }
#endif
}
