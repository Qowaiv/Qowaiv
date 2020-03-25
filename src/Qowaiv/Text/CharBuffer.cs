using System;

namespace Qowaiv.Text
{
    internal class CharBuffer: IEquatable<string>
    {
        public static readonly int NotFound = -1;

        private readonly char[] buffer;

        public CharBuffer(int capacity) => buffer = new char[capacity];

        public CharBuffer(string str) : this(str.Length)
        {
            Add(str);
        }

        public int Length { get; private set; }

        public char this[int index] => buffer[index];

        public bool Empty() => Length == 0;
        public bool NotEmpty() => Length != 0;

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
            for (var i = Length -1; i >= 0; i--)
            {
                if (buffer[i] == ch)
                {
                    return i;
                }
            }
            return NotFound;
        }

        /// <summary>Returns true if the buffer starts with the specified string.</summary>
        public bool StartsWith(string str)
        {
            if(str.Length > Length)
            {
                return false;
            }
            for(var i = 0; i < str.Length; i++)
            {
                if(str[i] != buffer[i])
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

        public CharBuffer Clear()
        {
            Length = 0;
            return this;
        }

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
    }
}
