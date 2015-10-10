using Qowaiv.Formatting;
using System;
using System.Diagnostics.CodeAnalysis;

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
		public static string ToString(this IFormattable formattable, FormattingArguments arguments)
		{
			if (formattable == null) { return null; }

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
		[SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "Qowaiv.Formatting.FormattingArgumentsCollection.#ctor",
			Justification = "Right culture selected by the default constructor.")]
		public static string ToString(this IFormattable formattable, FormattingArgumentsCollection argumentsCollection)
		{
			if (formattable == null) { return null; }

			return (argumentsCollection ?? new FormattingArgumentsCollection()).ToString(formattable);
		}
	}
}
