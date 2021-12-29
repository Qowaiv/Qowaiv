namespace Qowaiv;

public partial struct Percentage
{
    private static readonly string PercentFormat = "0.############################%";
    private static readonly string PerMilleFormat = "0.############################‰";
    private static readonly string PerTenThousendFormat = "0.############################‱";

    /// <summary>Gets a <see cref="NumberFormatInfo"/> based on the <see cref="IFormatProvider"/>.</summary>
    /// <remarks>
    /// Because the options for formatting and parsing percentages as provided 
    /// by the .NET framework are not sufficient, internally we use number
    /// settings. For parsing and formatting however we like to use the
    /// percentage properties of the <see cref="NumberFormatInfo"/> instead of
    /// the number properties, so we copy them for desired behavior.
    /// </remarks>
    [Pure]
    private static NumberFormatInfo GetNumberFormatInfo(IFormatProvider formatProvider)
    {
        var info = NumberFormatInfo.GetInstance(formatProvider);
        info = (NumberFormatInfo)info.Clone();
        info.NumberDecimalDigits = info.PercentDecimalDigits;
        info.NumberDecimalSeparator = info.PercentDecimalSeparator;
        info.NumberGroupSeparator = info.PercentGroupSeparator;
        info.NumberGroupSizes = info.PercentGroupSizes;
        return info;
    }

    internal static readonly Dictionary<SymbolPosition, decimal> Factors = new()
    {
        { SymbolPosition.None, 0.01m },
        { SymbolPosition.PercentBefore, 0.01m },
        { SymbolPosition.PercentAfter, 0.01m },
        { SymbolPosition.PerMilleBefore, 0.001m },
        { SymbolPosition.PerMilleAfter, 0.001m },
        { SymbolPosition.PerTenThousandBefore, 0.0001m },
        { SymbolPosition.PerTenThousandAfter, 0.0001m },
    };

    /// <summary>Resolves the format.</summary>
    [Pure]
    private static string Format(string? format, IFormatProvider formatProvider)
    {
        if (format is { Length: > 0 })
            return format switch
            {
                "PM" => PerMilleFormat,
                "PT" => PerTenThousendFormat,
                _ => format,
            };
        else return DefaultFormats.TryGetValue(formatProvider, out format)
                ? format
                : PercentFormat;
    }

    /// <summary>Gets the default format for different countries.</summary>
    private static readonly Dictionary<IFormatProvider, string> DefaultFormats = new()
    {
        { new CultureInfo("fr-FR"), "%0.############################" },
        { new CultureInfo("fa-IR"), "%0.############################" },
    };
}
