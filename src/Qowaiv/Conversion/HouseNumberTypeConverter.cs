namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a house number.</summary>
[Inheritable]
public class HouseNumberTypeConverter : NumericTypeConverter<HouseNumber, int>
{
    /// <inheritdoc />
    [Pure]
    protected override HouseNumber FromRaw(int raw) => HouseNumber.Create(raw);

    /// <inheritdoc />
    [Pure]
    protected override HouseNumber FromString(string? str, CultureInfo? culture) => HouseNumber.Parse(str, culture);

    /// <inheritdoc />
    [Pure]
    protected override int ToRaw(HouseNumber svo) => (int)svo;
}

