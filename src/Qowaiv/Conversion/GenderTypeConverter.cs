namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a Gender.</summary>
[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class GenderTypeConverter : SvoTypeConverter<Gender>
{
    /// <inheritdoc/>
    [Pure]
    protected override Gender FromString(string? str, CultureInfo? culture) => Gender.Parse(str, culture);
}
