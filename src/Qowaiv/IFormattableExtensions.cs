using Qowaiv.Formatting;
using System;
using System.Diagnostics.Contracts;

namespace Qowaiv
{
    /// <summary>Extensions on System.IFormattable.</summary>
    public static class IFormattableExtensions
    {
        /// <summary>Formats the object using the formatting arguments.</summary>
        /// <param name="formattable">
        /// The object to format.
        /// </param>
        /// <param name="arguments">
        /// The formatting arguments
        /// </param>
        /// <returns>
        /// A formatted string representing the object.
        /// </returns>
        [Pure]
        public static string ToString(this IFormattable formattable, FormattingArguments arguments)
        {
            if (formattable == null)
            {
#pragma warning disable S2225
                // "ToString()" method should not return null
                // if the origin is null, it should not become string.Empty.
                return null;
#pragma warning restore S2225
            }
            return arguments.ToString(formattable);
        }

        /// <summary>Formats the object using the formatting arguments collection.</summary>
        /// <param name="formattable">
        /// The object to format.
        /// </param>
        /// <param name="argumentsCollection">
        /// The formatting arguments collection.
        /// </param>
        /// <returns>
        /// A formatted string representing the object.
        /// </returns>
        [Pure]
        public static string ToString(this IFormattable formattable, FormattingArgumentsCollection argumentsCollection)
        {
            if (formattable == null)
            {
#pragma warning disable S2225 
                // "ToString()" method should not return null
                // if the origin is null, it should not become string.Empty.
                return null;
#pragma warning restore S2225
            }

            return (argumentsCollection ?? new FormattingArgumentsCollection()).ToString(formattable);
        }
    }
}
