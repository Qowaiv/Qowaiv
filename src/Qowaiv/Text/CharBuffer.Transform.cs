using System;
using System.Globalization;

namespace Qowaiv.Text
{
    internal partial class CharBuffer
    {
        /// <summary>Adds an other <see cref="CharBuffer"/> to the buffer.</summary>
        public CharBuffer Add(CharBuffer other)
        {
            Array.Copy(
                sourceArray: other.buffer,
                sourceIndex: other.start,
                destinationArray: buffer,
                destinationIndex: end,
                length: other.Length);
            end += other.Length;
            return this;
        }

        /// <summary>Adds a <see cref="string"/> to the buffer.</summary>
        public CharBuffer Add(string str)
        {
            var s = str ?? string.Empty;
            foreach (var ch in s)
            {
                Add(ch);
            }
            return this;
        }

        /// <summary>Adds a <see cref="char"/> to the buffer.</summary>
        public CharBuffer Add(char ch)
        {
            buffer[end++] = ch;
            return this;
        }

        /// <summary>Adds a lowercase <see cref="char"/> to the buffer.</summary>
        public CharBuffer AddLower(char ch) => Add(char.ToLowerInvariant(ch));

        /// <summary>Clears the buffer.</summary>
        public CharBuffer Clear()
        {
            end = start;
            return this;
        }

        /// <summary>Removes all instances of <param name="str"/> from the buffer.</summary>
        public CharBuffer Remove(string str)
        {
            var match = 0;
            for (var i = 0; i < Length; i++)
            {
                if(str[match] == this[i])
                {
                    match++;
                    
                    if (match == str.Length)
                    {
                        i -= match - 1;
                        RemoveRange(i, match);
                        match = 0;
                    }
                }
                else
                {
                    match = 0;
                }
            }
            return this;
        }

        /// <summary>Removes a specified length from the start of the buffer.</summary>
        public CharBuffer RemoveFromStart(int length)
        {
            start += length;
            return this;
        }

        /// <summary>Removes a specified length from the end of the buffer.</summary>
        public CharBuffer RemoveFromEnd(int length)
        {
            end -= length;
            return this;
        }

        /// <summary>Removes a specified range from the buffer.</summary>
        public CharBuffer RemoveRange(int index, int length)
        {
            for (var i = index + length; i < Length; i++)
            {
                this[i - length] = this[i];
            }
            end -= length;
            return this;
        }

        /// <summary>Removes all white space from the buffer.</summary>
        public CharBuffer RemoveWhiteSpace() => ClearChars(IsWhiteSpace);

        /// <summary>Removes all markup (spacing, dots, dashes, underscores) from the buffer.</summary>
        public CharBuffer RemoveMarkup() => ClearChars(IsMarkup);

        private CharBuffer ClearChars(Func<char, bool> applies)
        {
            if (NotEmpty() && applies(Last()))
            {
                return RemoveFromEnd(1).ClearChars(applies);
            }
            else if (NotEmpty() && applies(First()))
            {
                return RemoveFromStart(1).ClearChars(applies);
            }
            else
            {
                for (var i = Length - 2; i >= 0; i--)
                {
                    if (applies(this[i]))
                    {
                        for (var p = i; p < Length - 1; p++)
                        {
                            this[p] = this[p + 1];
                        }
                        end -= 1;
                    }
                }
            }
            return this;
        }

        /// <summary>Removes all leading and trailing white-space characters from the buffer.</summary>
        public CharBuffer Trim() => TrimRight().TrimLeft();

        /// <summary>Removes all leading and trailing white-space characters from the left side of the buffer.</summary>
        public CharBuffer TrimLeft()
        {
            while(NotEmpty() && IsWhiteSpace(First()))
            {
                start++;
            }
            return this;
        }

        /// <summary>Removes all leading and trailing white-space characters from the right side of the buffer.</summary>
        public CharBuffer TrimRight()
        {
            while (NotEmpty() && IsWhiteSpace(Last()))
            {
                end--;
            }
            return this;
        }

        /// <summary>Transforms all characters of to the buffer to their uppercase variant.</summary>
        public CharBuffer Uppercase()
        {
            for (var i = start; i < end; i++)
            {
                buffer[i] = char.ToUpper(buffer[i], CultureInfo.InvariantCulture);
            }
            return this;
        }

        /// <summary>Unifies the buffer applying <see cref="RemoveMarkup"/>, <see cref="Uppercase"/>
        /// and <see cref="ToNonDiacritic(string)"/>.
        /// </summary>
        public CharBuffer Unify()
            => RemoveMarkup()
            .Uppercase()
            .ToNonDiacritic();

        private static bool IsWhiteSpace(char ch) => char.IsWhiteSpace(ch);

        private static bool IsMarkup(char ch) => IsWhiteSpace(ch) || markup.IndexOf(ch) != NotFound;

        private static readonly string markup = "-._"
            + (char)0x00B7 // middle dot
            + (char)0x22C5 // dot operator
            + (char)0x2202 // bullet
            + (char)0x2012 // figure dash / minus
            + (char)0x2013 // en dash
            + (char)0x2014 // em dash
            + (char)0x2015 // horizontal bar
        ;
    }
}
