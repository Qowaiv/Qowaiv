using System;

namespace Qowaiv.Text
{
    /// <summary> is a group of similar binary-to-text encoding schemes that
    /// represent binary data in an ASCII string format by translating it into
    /// a radix-32 representation.
    /// </summary>
    public static class Base32
    {
        private const int BitPerByte = 8;
        private const int BitPerChar = 5;
        private const int BitShift = BitPerByte - BitPerChar;
        private const int BitsMask = 31;

        private const string UpperCaseBitChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        private const string LowerCaseBitChars = "abcdefghijklmnopqrstuvwxyz234567";

        /// <summary>A lookup where the index is the <see cref="int"/> representation of the <see cref="char"/>.</summary>
        private static readonly byte[] CharValues = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 26, 27, 28, 29, 30, 31, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 255, 255, 255, 255, 255, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
        /// <summary>The 'z' is the highest <see cref="char"/> that can be found in the <see cref="CharValues"/>.</summary>
        private const char MaxChar = 'z';

        /// <summary>Represents a byte array as a <see cref="string"/>.</summary>
        /// <param name="bytes">
        /// The bytes to represent as Base32 string.
        /// </param>
        /// <remarks>
        /// Uppercase by default.
        /// </remarks>
        public static string ToString(byte[] bytes) => ToString(bytes, false);

        /// <summary>Represents a byte array as a <see cref="string"/>.</summary>
        /// <param name="bytes">
        /// The bytes to represent as Base32 string.
        /// </param>
        /// <param name="lowerCase">
        /// An indicator to specify lower case or upper case.
        /// </param>
        public static string ToString(byte[] bytes, bool lowerCase)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return string.Empty;
            }

            var lookup = lowerCase ? LowerCaseBitChars : UpperCaseBitChars;

            // Determine the string size.
            var bitLength = bytes.Length << 3;
            var chars = new char[1 + (bitLength - 1) / BitPerChar];

            var indexChars = 0;
            var indexBytes = 0;
            int buffer = bytes[indexBytes++];
            var overFlow = BitShift;

            while (indexChars < chars.Length)
            {
                // Get the 5 bits, from the buffer to chars.
                // A negative overflow can only occur at the end.
                var bits = overFlow < 0 ? (buffer << -overFlow) : (buffer >> overFlow);
                chars[indexChars++] = lookup[bits & BitsMask];

                // If the buffer is too small, and there are bytes left.
                if (overFlow < BitPerChar && indexBytes < bytes.Length)
                {
                    // Make place at the beginning of the buffer.
                    buffer <<= BitPerByte;
                    // Add the bits.
                    buffer |= bytes[indexBytes++];
                    // Update the overflow.
                    overFlow += BitShift;
                }
                else
                {
                    overFlow -= BitPerChar;
                }
            }
            return new string(chars);
        }

        /// <summary>Gets the bytes for a given Base32 string.</summary>
        /// <param name="s">
        /// The Base32 string
        /// </param>
        /// <returns>
        /// A byte array representation of the Base32 string.
        /// </returns>
        /// <exception cref="FormatException">
        /// If the string is not a valid Base32 string.
        /// </exception>
        public static byte[] GetBytes(string s)
        {
            if (TryGetBytes(s, out byte[] bytes))
            {
                return bytes;
            }
            throw new FormatException(QowaivMessages.FormatExceptionBase32);
        }

        /// <summary>Tries to get the corresponding bytes of the Base32 string.</summary>
        /// <param name="s">
        /// The string to convert.
        /// </param>
        /// <param name="bytes">
        /// The bytes represented by the Base32 string.
        /// </param>
        /// <returns>
        /// True if the string is a Base32 string, otherwise false.
        /// </returns>
        public static bool TryGetBytes(string s, out byte[] bytes)
        {
            if (string.IsNullOrEmpty(s))
            {
                bytes = new byte[0];
                return true;
            }

            // Determine the byte size.
            var bitLength = s.Length * BitPerChar;
            bytes = new byte[bitLength >> 3];

            var buffer = 0;
            var bufferLength = 0;
            var index = 0;

            // Loop through all chars.
            foreach (var ch in s)
            {
                // the char is not a valid base32 char.
                if (ch > MaxChar)
                {
                    bytes = new byte[0];
                    return false;
                }
                var charValue = CharValues[ch];
                // the char is not a valid base32 char, although it is in the lookup.
                if (charValue == byte.MaxValue)
                {
                    bytes = new byte[0];
                    return false;
                }

                // Create 5 bits at the beginning for the new char value.
                buffer <<= BitPerChar;
                // Add the new bits.
                buffer |= charValue;
                // Update the buffer length.
                bufferLength += BitPerChar;

                // If the buffer is big enough to represent a byte.
                if (bufferLength >= BitPerByte)
                {
                    // The byte to add are at the end of the buffer.
                    var bits = buffer >> (bufferLength - BitPerByte);

                    bytes[index++] = (byte)bits;
                    bufferLength -= BitPerByte;
                }
            }
            return true;
        }
    }
}
