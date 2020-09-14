using Qowaiv.Formatting;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Qowaiv.Text
{
    internal sealed class CharBuffer : IEquatable<string>
    {
        internal static readonly int NotFound = -1;

        private readonly char[] buffer;

        public CharBuffer(int capacity) => buffer = new char[capacity];

        public CharBuffer(string str) : this(str.Length)
        {
            Add(str);
        }

        public int Length { get; private set; }

        public char this[int index] => buffer[index];

        public bool IsEmpty() => Length == 0;
        public bool NotEmpty() => Length != 0;

        public bool IsUnknown(IFormatProvider provider) 
            => Unknown.IsUnknown(ToString(), provider as CultureInfo);

        public bool Matches(Regex regex) => regex.IsMatch(ToString());

        public char First() => buffer[0];
        public char Last() => buffer[Length - 1];

        public CharBuffer Add(CharBuffer other)
        {
            Array.Copy(other.buffer, 0, buffer, Length, other.Length);
            Length += other.Length;
            return this;
        }

        public CharBuffer Add(string str)
        {
            foreach (var ch in str)
            {
                Add(ch);
            }
            return this;
        }
        public CharBuffer Add(char ch)
        {
            buffer[Length++] = ch;
            return this;
        }
        public CharBuffer AddLower(char ch) => Add(char.ToLowerInvariant(ch));

        /// <summary>Gets the index of the <see cref="char"/> in the buffer.</summary>
        /// <returns>
        /// -1 if not found, otherwise the index of the <see cref="char"/>.
        /// </returns>
        public int IndexOf(char ch)
        {
            for (var i = 0; i < Length; i++)
            {
                if (buffer[i] == ch)
                {
                    return i;
                }
            }
            return NotFound;
        }

        /// <summary>Gets the last index of the <see cref="char"/> in the buffer.</summary>
        /// <returns>
        /// -1 if not found, otherwise the index of the <see cref="char"/>.
        /// </returns>
        public int LastIndexOf(char ch)
        {
            for (var i = Length - 1; i >= 0; i--)
            {
                if (buffer[i] == ch)
                {
                    return i;
                }
            }
            return NotFound;
        }

        public bool EndOfBuffer(int index) => index >= Length - 1;

        public bool StartsWith(string str)
        {
            if (str.Length > Length)
            {
                return false;
            }
            for (var i = 0; i < str.Length; i++)
            {
                if (buffer[i] != str[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>Counts the occurrences of the <see cref="char"/> in the buffer.</summary>
        public int Count(char ch)
        {
            var count = 0;
            for (var i = 0; i < Length; i++)
            {
                if (buffer[i] == ch)
                {
                    count++;
                }
            }
            return count;
        }

        public CharBuffer Trim()
        {
            return TrimRight()
                .TrimLeft();
        }
        public CharBuffer TrimLeft()
        {
            for (var trim = 0; trim < Length; trim++)
            {
                if (!char.IsWhiteSpace(buffer[trim]))
                {
                    RemoveRange(0, trim);
                    return this;
                }
            }
            return this;
        }
        public CharBuffer TrimRight()
        {
            for (var trim = Length - 1; trim > -1; trim--)
            {
                if (!char.IsWhiteSpace(buffer[trim]))
                {
                    Length = trim + 1;
                    return this;
                }
            }
            return this;
        }
       
        public CharBuffer RemoveFromEnd(int length)
        {
            Length -= length;
            return this;
        }

        public CharBuffer RemoveRange(int index, int length)
        {
            if (length == 0)
            {
                return this;
            }
            for (var i = index + length; i < Length; i++)
            {
                buffer[i - length] = buffer[i];
            }
            Length -= length;
            return this;
        }

        public CharBuffer ClearSpacing() => ClearChars(IsWhiteSpace);

        public CharBuffer ClearMarkup() => ClearChars(IsMarkup);

        private CharBuffer ClearChars(Func<char, bool> applies)
        {
            if (NotEmpty() && applies(Last()))
            {
                return RemoveFromEnd(1).ClearSpacing();
            }
            else
            {
                for (var i = Length - 2; i >= 0; i--)
                {
                    if (applies(this[i]))
                    {
                        for (var p = i; p < Length - 1; p++)
                        {
                            buffer[p] = buffer[p + 1];
                        }
                        Length -= 1;
                    }
                }
            }
            return this;
        }

        public CharBuffer Clear()
        {
            Length = 0;
            return this;
        }

        public CharBuffer Uppercase()
        {
            for (var i = 0; i < Length; i++)
            {
                buffer[i] = char.ToUpper(buffer[i], CultureInfo.InvariantCulture);
            }
            return this;
        }

        public CharBuffer Unify()
        {
            if(IsEmpty())
            {
                return this;
            }
            var charBuffer = new CharBuffer(Length * 2);

            for (var i = 0; i < Length; i++)
            {
                var ch = buffer[i];

                var index = StringFormatter.DiacriticSearch.IndexOf(ch);
                if (index == NotFound)
                {
                    if (StringFormatter.DiacriticLookup.TryGetValue(ch, out string chs))
                    {
                        charBuffer.Add(chs);
                    }
                    else
                    {
                        charBuffer.Add(ch);
                    }
                }
                else
                {
                    charBuffer.Add(StringFormatter.DiacriticReplace[index]);
                }
            }
            return charBuffer
                .ClearSpacing()
                .ClearMarkup()
                .Uppercase();
        }

        public string Substring(int startIndex) => new string(buffer, startIndex, Length - startIndex);

        public string Substring(int startIndex, int length) => new string(buffer, startIndex, length);

        /// <inheritdoc />
        public bool Equals(string other) => Equals(other, false);
        
        public bool Equals(string other, bool ignoreCase)
        {
            if (Length != other.Length)
            {
                return false;
            }
            for (var i = 0; i < Length; i++)
            {
                if (buffer[i] != other[i])
                {
                    if (ignoreCase && char.ToUpperInvariant(buffer[i]) == char.ToUpperInvariant(other[i]))
                    {
                        continue;
                    }
                    return false;
                }
            }
            return true;
        }

        public static implicit operator string(CharBuffer buffer) => buffer?.ToString();

        public override string ToString() => new string(buffer, 0, Length);

        private static bool IsWhiteSpace(char ch) => char.IsWhiteSpace(ch);
        
        private static bool IsMarkup(char ch) => markup.IndexOf(ch) != NotFound;

        private static readonly string markup = "-+._"
            + (char)0x00B7 // middle dot
            + (char)0x22C5 // dot operator
            + (char)0x2202 // bullet
            + (char)0x2012 // figure dash / minus
            + (char)0x2013 // en dash
            + (char)0x2014 // em dash
            + (char)0x2015 // horizontal bar
        ;
    }

    internal static class CharrBufferExtensions
    {
        public static CharBuffer Buffer(this string str)
            => str is null
            ? new CharBuffer(0)
            : new CharBuffer(str);
    }
}
