#if NET6_0_OR_GREATER

using Qowaiv.Chemistry;

namespace Qowaiv.Json.Chemistry;

/// <summary>Provides a JSON conversion for a CAS registry.</summary>
[Inheritable]
public class CasRegistryNumberJsonConverter : SvoJsonConverter<CasRegistryNumber>
{
    /// <inheritdoc />
    [Pure]
    protected override CasRegistryNumber FromJson(string? json) => CasRegistryNumber.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override CasRegistryNumber FromJson(long json) => CasRegistryNumber.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(CasRegistryNumber svo) => svo.ToJson();
}

#endif
