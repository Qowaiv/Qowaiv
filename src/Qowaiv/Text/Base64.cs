using System;
using System.Diagnostics.CodeAnalysis;

namespace Qowaiv.Text
{
    /// <summary> is a group of similar binary-to-text encoding schemes that
    /// represent binary data in an ASCII string format by translating it into
    /// a radix-64 representation. The term Base64 originates from MIME.
    /// (RFC 1341, since made obsolete by RFC 2045) 
    /// </summary>
    public static class Base64
	{
		/// <summary>Represents a byte array as a <see cref="string"/>.</summary>
		public static string ToString(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0) { return string.Empty; }
			return Convert.ToBase64String(bytes);
		}
		
		/// <summary>Tries to get the corresponding bytes of the Base64 string.</summary>
		/// <param name="s">
		/// The string to convert.
		/// </param>
		/// <param name="bytes">
		/// The bytes represented by the Base64 string.
		/// </param>
		/// <returns>
		/// True if the string is a Base64 string, otherwise false.
		/// </returns>
		/// <remarks>
		/// If the conversion fails,  bytes is an empty byte array, not null.
		/// </remarks>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
			Justification="We don't want to redo the logic of ConvertFromBase64String.")]
		public static bool TryGetBytes(string s, out byte[] bytes)
		{
			if (string.IsNullOrEmpty(s))
			{
				bytes = new byte[0];
				return true;
			}
			try
			{
				bytes = Convert.FromBase64String(s);
				return true;
			}
			catch 
			{
				bytes = new byte[0];
				return false;
			}
		}
	}
}
