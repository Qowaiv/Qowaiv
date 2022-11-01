using Qowaiv.Web;

namespace Qowaiv.Conversion.Web;

/// <summary>Provides a conversion for an Internet media type.</summary>
[Inheritable]
public class InternetMediaTypeTypeConverter : SvoTypeConverter<InternetMediaType>
{
    /// <inheritdoc/>
    [Pure]
    protected override InternetMediaType FromString(string? str, CultureInfo? culture) => InternetMediaType.Parse(str);
}
