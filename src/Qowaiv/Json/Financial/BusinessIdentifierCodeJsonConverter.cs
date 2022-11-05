#if NET5_0_OR_GREATER

using Qowaiv.Financial;

namespace Qowaiv.Json.Financial;

/// <summary>Provides a JSON conversion for a BIC.</summary>
[Inheritable]
public class BusinessIdentifierCodeJsonConverter : SvoJsonConverter<BusinessIdentifierCode>
{
    /// <inheritdoc />
    [Pure]
    protected override BusinessIdentifierCode FromJson(string? json) => BusinessIdentifierCode.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(BusinessIdentifierCode svo) => svo.ToJson();
}

#endif
