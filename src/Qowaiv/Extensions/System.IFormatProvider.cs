namespace System
{
    internal static class QowaivFormatProviderExtensions
    {
        [Pure]
        public static string NegativeSign(this IFormatProvider? provider)
            => (provider is CultureInfo culture
            ? culture.NumberFormat.NegativeSign
            : provider?.GetFormat<NumberFormatInfo>()?.NegativeSign) ?? "-";
    }
}
