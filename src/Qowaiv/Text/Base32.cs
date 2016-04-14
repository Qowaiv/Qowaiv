using System;
using System.Text;

namespace Qowaiv.Text
{
    /// <summary> is a group of similar binary-to-text encoding schemes that
    /// represent binary data in an ASCII string format by translating it into
    /// a radix-32 representation.
    /// </summary>
    /// <remarks>
    /// Codes originates from <see cref="http://scottless.com/blog/archive/2014/02/15/base32-encoder-and-decoder-in-c.aspx"/>
    /// </remarks>
    public static class Base32
    {
        /// <summary>
        /// Size of the regular byte in bits
        /// </summary>
        private const int InByteSize = 8;

        /// <summary>
        /// Size of converted byte in bits
        /// </summary>
        private const int OutByteSize = 5;

        /// <summary>Gets the last five bit of the int.</summary>
        /// <remarks>
        /// 0x001F = 00011111B
        /// </remarks>
        private const int OverflowMask = 0x001F;

        /// <summary>The lookup for <see cref="ToString(byte[])"/>.</summary>
        private const string LookupToString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        /// <summary>The lookup for <see cref="ToString(byte[])"/>.</summary>
        private const string LookupGetBytes = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz223344556677";

        /// <summary>Represents a byte array as a <see cref="string"/>.</summary>
        public static string ToString(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) { return string.Empty; }

            // Prepare container for the final value
            var buffer = new StringBuilder(1 + bytes.Length * InByteSize / OutByteSize);

            var position = 0;

            // Offset inside a single byte that <bytesPosition> points to (from left to right)
            // 0 - highest bit, 7 - lowest bit
            var subposition = 0;

            // Byte to look up in the dictionary
            var base32Byte = 0;

            // The number of bits filled in the current output byte
            var base32BytePosition = 0;

            // Iterate through input buffer until we reach past the end of it
            while (position < bytes.Length)
            {
                // Calculate the number of bits we can extract out of current input byte to fill missing bits in the output byte
                var bitsAvailableInByte = Math.Min(InByteSize - subposition, OutByteSize - base32BytePosition);

                // Make space in the output byte
                base32Byte <<= bitsAvailableInByte;

                // Extract the part of the input byte and move it to the output byte
                base32Byte |= (byte)(bytes[position] >> (InByteSize - (subposition + bitsAvailableInByte)));

                // Update current sub-byte position
                subposition += bitsAvailableInByte;

                // Check overflow
                if (subposition >= InByteSize)
                {
                    // Move to the next byte
                    position++;
                    subposition = 0;
                }

                // Update current base32 byte completion
                base32BytePosition += bitsAvailableInByte;

                // Check overflow or end of input array
                if (base32BytePosition >= OutByteSize)
                {
                    // Drop the overflow bits
                    base32Byte &= 0x1F;  // 0x1F = 00011111 in binary

                    // Add current Base32 byte and convert it to character
                    buffer.Append(LookupToString[base32Byte]);

                    // Move to the next byte
                    base32BytePosition = 0;
                }
            }

            // Check if we have a remainder
            if (base32BytePosition > 0)
            {
                // Move to the right bits
                base32Byte <<= (OutByteSize - base32BytePosition);
                // Add current Base32 byte and convert it to character
                buffer.Append(LookupToString[base32Byte & OverflowMask]);
            }
            return buffer.ToString();
        }

        /// <summary>Gets the bytes for a given Base32 string. </summary>
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
            byte[] bytes;

            if (TryGetBytes(s, out bytes))
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

            // Prepare output byte array
            bytes = new byte[s.Length * OutByteSize / InByteSize];
            
            // Position in the string
            int base32Position = 0;

            // Offset inside the character in the string
            int base32SubPosition = 0;

            // Position within outputBytes array
            int outputBytePosition = 0;

            // The number of bits filled in the current output byte
            int outputByteSubPosition = 0;

            // Normally we would iterate on the input array but in this case we actually iterate on the output array
            // We do it because output array doesn't have overflow bits, while input does and it will cause output array overflow if we don't stop in time
            while (outputBytePosition < bytes.Length)
            {
                // Look up current character in the dictionary to convert it to byte
                int currentBase32Byte = LookupGetBytes.IndexOf(s[base32Position]);

                // Check if found
                if (currentBase32Byte < 0)
                {
                    bytes = new byte[0];
                    return false;
                }
                else
                {
                    currentBase32Byte >>= 1;
                }

                // Calculate the number of bits we can extract out of current input character to fill missing bits in the output byte
                int bitsAvailableInByte = Math.Min(OutByteSize - base32SubPosition, InByteSize - outputByteSubPosition);

                // Make space in the output byte
                bytes[outputBytePosition] <<= bitsAvailableInByte;

                // Extract the part of the input character and move it to the output byte
                bytes[outputBytePosition] |= (byte)(currentBase32Byte >> (OutByteSize - (base32SubPosition + bitsAvailableInByte)));

                // Update current sub-byte position
                outputByteSubPosition += bitsAvailableInByte;

                // Check overflow
                if (outputByteSubPosition >= InByteSize)
                {
                    // Move to the next byte
                    outputBytePosition++;
                    outputByteSubPosition = 0;
                }

                // Update current base32 byte completion
                base32SubPosition += bitsAvailableInByte;

                // Check overflow or end of input array
                if (base32SubPosition >= OutByteSize)
                {
                    // Move to the next character
                    base32Position++;
                    base32SubPosition = 0;
                }
            }
            return true;
        }
    }
}
