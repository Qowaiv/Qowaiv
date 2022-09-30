#if NET5_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a email address.</summary>
public sealed class EmailAddressJsonConverter : SvoJsonConverter<EmailAddress>
{
    /// <inheritdoc />
    [Pure]
    protected override EmailAddress FromJson(string? json) => EmailAddress.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(EmailAddress svo) => svo.ToJson();
}

#endif
