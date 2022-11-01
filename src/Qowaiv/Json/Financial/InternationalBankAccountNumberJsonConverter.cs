#if NET5_0_OR_GREATER

using Qowaiv.Financial;

namespace Qowaiv.Json.Financial;

/// <summary>Provides a JSON conversion for an IBAN.</summary>
[Inheritable]
public class InternationalBankAccountNumberJsonConverter : SvoJsonConverter<InternationalBankAccountNumber>
{
    /// <inheritdoc />
    [Pure]
    protected override InternationalBankAccountNumber FromJson(string? json) => InternationalBankAccountNumber.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(InternationalBankAccountNumber svo) => svo.ToJson();
}

#endif
