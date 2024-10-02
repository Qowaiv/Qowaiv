using Qowaiv.Chemistry;

namespace Qowaiv.Conversion.Chemistry;

/// <summary>Provides a conversion for a house number.</summary>
[Inheritable]
public class CasRegistryNumberTypeConverter : NumericTypeConverter<CasRegistryNumber, long>
{
    /// <inheritdoc />
    [Pure]
    protected override CasRegistryNumber FromRaw(long raw) => CasRegistryNumber.Create(raw);

    /// <inheritdoc />
    [Pure]
    protected override CasRegistryNumber FromString(string? str, CultureInfo? culture) => CasRegistryNumber.Parse(str, culture);

    /// <inheritdoc />
    [Pure]
    protected override long ToRaw(CasRegistryNumber svo) => (long)svo;
}
