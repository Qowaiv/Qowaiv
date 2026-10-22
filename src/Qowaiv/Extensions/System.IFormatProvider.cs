namespace System;

internal static class QowaivFormatProviderExtensions
{
    extension(IFormatProvider? provider)
    {
        [Pure]
        public string NegativeSign()
            => (provider is CultureInfo culture
            ? culture.NumberFormat.NegativeSign
            : provider?.GetFormat<NumberFormatInfo>()?.NegativeSign) ?? "-";

        [Pure]
        public string PositiveSign()
            => (provider is CultureInfo culture
            ? culture.NumberFormat.PositiveSign
            : provider?.GetFormat<NumberFormatInfo>()?.PositiveSign) ?? "+";
    }
}
