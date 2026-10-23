namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="IFormatProvider" />.</summary>
public static class QowaivIFormatProvider
{
    /// <typeparam name="TFormat">
    /// An object that specifies the type of format object to return.
    /// </typeparam>
    extension<TFormat>(IFormatProvider provider)
    {
        /// <summary>Returns an object that provides formatting services for the specified type.</summary>
        /// <returns>
        /// An instance of the object specified by formatType, if the System.IFormatProvider
        /// implementation can supply that type of object; otherwise, null.
        /// </returns>
        [Pure]
        public TFormat? GetFormat()
        {
            Guard.NotNull(provider);
            return (TFormat?)provider.GetFormat(typeof(TFormat));
        }
    }
}
