using System.Diagnostics.Contracts;

namespace Qowaiv.Text
{
    internal partial class CharBuffer
    {
        /// <summary>Counts the occurrences of the <see cref="char"/> in the buffer.</summary>
        [Pure]
        public int Count(char ch)
        {
            var count = 0;
            for (var i = start; i < end; i++)
            {
                if (buffer[i] == ch)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>Counts the occurrences of the <see cref="string"/> in the buffer.</summary>
        [Pure]
        public int Count(string str)
        {
            var count = 0;
            var match = 0;
            for (var i = start; i < end; i++)
            {
                if (buffer[i] == str[match])
                {
                    match++;
                    if(match == str.Length)
                    {
                        count++;
                        match = 0;
                    }
                }
                else
                {
                    match = 0;
                }
            }
            return count;
        }

        /// <summary>Counts the occurrences of the <see cref="string"/> in the buffer.</summary>
        [Pure]
        public bool Contains(string str)
        {
            var match = 0;
            for (var i = start; i < end; i++)
            {
                if (buffer[i] == str[match])
                {
                    match++;
                    if (match == str.Length)
                    {
                        return true;
                    }
                }
                else
                {
                    match = 0;
                }
            }
            return false;
        }

        /// <summary>Gets the index of the <see cref="char"/> in the buffer.</summary>
        /// <returns>
        /// -1 if not found, otherwise the index of the <see cref="char"/>.
        /// </returns>
        [Pure]
        public int IndexOf(char ch)
        {
            for (var i = start; i < end; i++)
            {
                if (buffer[i] == ch)
                {
                    return i - start;
                }
            }
            return NotFound;
        }

        /// <summary>Gets the last index of the <see cref="char"/> in the buffer.</summary>
        /// <returns>
        /// -1 if not found, otherwise the index of the <see cref="char"/>.
        /// </returns>
        [Pure]
        public int LastIndexOf(char ch)
        {
            for (var i = end - 1; i >= start; i--)
            {
                if (buffer[i] == ch)
                {
                    return i - start;
                }
            }
            return NotFound;
        }

        /// <summary>Returns true if buffer starts with the specified string.</summary>
        [Pure]
        public bool StartsWith(string str, bool ignoreCase = false)
        {
            if (str.Length > Length)
            {
                return false;
            }
            return ignoreCase 
                ? StartsWithCaseInsensitve(str)
                : StartsWithCaseSensitve(str);
        }

        [Pure]
        private bool StartsWithCaseSensitve(string str)
        {
            for (var i = 0; i < str.Length; i++)
            {
                if (this[i] != str[i]) { return false; }
            }
            return true;
        }
        [Pure]
        private bool StartsWithCaseInsensitve(string str)
        {
            for (var i = 0; i < str.Length; i++)
            {
                if (char.ToUpperInvariant(this[i]) != char.ToUpperInvariant(str[i])) { return false; }
            }
            return true;
        }

        [Pure]
        public bool EndsWith(string str)
        {
            if (str.Length > Length)
            {
                return false;
            }

            var offset = Length - str.Length;
            for (var i = 0; i < str.Length; i++)
            {
                if (this[offset + i] != str[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>Retrieves a substring from the buffer..</summary>
        [Pure]
        public string Substring(int startIndex) => new(buffer, startIndex + start, Length - startIndex);

        /// <summary>Retrieves a substring from the buffer..</summary>
        [Pure]
        public string Substring(int startIndex, int length) => new(buffer, startIndex + start, length);

        /// <inheritdoc />
        [Pure]
        public override string ToString() => new(buffer, start, Length);

        /// <summary>Implicitly casts a buffer to a <see cref="string"/>.</summary>
        public static implicit operator string(CharBuffer buffer) => buffer?.ToString();
    }
}
