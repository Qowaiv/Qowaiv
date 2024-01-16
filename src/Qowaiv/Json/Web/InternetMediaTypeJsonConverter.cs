﻿#if NET6_0_OR_GREATER

using Qowaiv.Web;

namespace Qowaiv.Json.Web;

/// <summary>Provides a JSON conversion for an Internet media type.</summary>
[Inheritable]
public class InternetMediaTypeJsonConverter : SvoJsonConverter<InternetMediaType>
{
    /// <inheritdoc />
    [Pure]
    protected override InternetMediaType FromJson(string? json) => InternetMediaType.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(InternetMediaType svo) => svo.ToJson();
}

#endif
