namespace Qowaiv.Internals;

internal partial class ConventionBasedSerializer<TSvo>
{
    /// <summary>Initializes a new instance of the <see cref="ConventionBasedSerializer{TSvo}"/> class.</summary>
#pragma warning disable CS8618 // Initialize does actually initialize all.
    public ConventionBasedSerializer() => Initialize();
#pragma warning restore CS8618

    /// <summary>Returns true if <typeparamref name="TSvo"/> is supported.</summary>
    public bool TypeIsSupported => fromJsonString != null;

    /// <summary>Serializes the <paramref name="svo"/> to a JSON node.</summary>
    /// <param name="svo">
    /// The object to serialize.
    /// </param>
    /// <returns>
    /// The serialized JSON node.
    /// </returns>
    [Pure]
    public object? ToJson(TSvo svo) => toJsonObject(svo);

    /// <summary>Deserializes the JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The actual instance of <typeparamref name="TSvo"/>.
    /// </returns>
    [Pure]
    public TSvo FromJson(string? json) => fromJsonString(json);

    /// <summary>Deserializes the JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The actual instance of <typeparamref name="TSvo"/>.
    /// </returns>
    [Pure]
    public TSvo FromJson(double json) => fromJsonDouble(json);

    /// <summary>Deserializes the JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The actual instance of <typeparamref name="TSvo"/>.
    /// </returns>
    [Pure]
    public TSvo FromJson(long json) => fromJsonLong(json);

    /// <summary>Deserializes the JSON boolean.</summary>
    /// <param name="json">
    /// The number boolean to deserialize.
    /// </param>
    /// <returns>
    /// The actual instance of <typeparamref name="TSvo"/>.
    /// </returns>
    [Pure]
    public TSvo FromJson(bool json) => fromJsonBool(json);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Func<string?, TSvo> fromJsonString;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Func<double, TSvo> fromJsonDouble;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Func<long, TSvo> fromJsonLong;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Func<bool, TSvo> fromJsonBool;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Func<TSvo, object?> toJsonObject;
}
