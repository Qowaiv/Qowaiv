#if NET8_0_OR_GREATER

using Qowaiv.IO;

namespace Qowaiv.Json.IO;

/// <summary>Provides a JSON conversion for a stream size.</summary>
[Inheritable]
public class StreamSizeJsonConverter : SvoJsonConverter<StreamSize>
{
    /// <inheritdoc />
    [Pure]
    protected override StreamSize FromJson(string? json) => StreamSize.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override StreamSize FromJson(long json) => StreamSize.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override StreamSize FromJson(double json) => StreamSize.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(StreamSize svo) => svo.ToJson();
}

#endif
