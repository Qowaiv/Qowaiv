namespace System.ComponentModel;

internal static class QowaivTypeConverterExtensions
{
    extension(TypeConverter converter)
    {
        [Pure]
        public T ConvertFromCultureInvariant<T>(object value) => (T)converter.ConvertFrom(null, CultureInfo.InvariantCulture, value)!;
    }
}
