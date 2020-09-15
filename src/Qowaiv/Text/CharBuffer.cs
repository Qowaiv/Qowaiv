using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Qowaiv.Text
{
    internal sealed partial class CharBuffer : IEquatable<string>
    {
        internal static readonly int NotFound = -1;

        private readonly char[] buffer;
        private int start;
        private int end;

        /// <summary>Initializes a new instance of the <see cref="CharBuffer"/> class.</summary>
        public CharBuffer(int capacity) => buffer = new char[capacity];

        /// <summary>Initializes a new instance of the <see cref="CharBuffer"/> class.</summary>
        public CharBuffer(string str) : this(str.Length) => Add(str);

        /// <summary>Gets the length of the buffer.</summary>
        public int Length => end - start;

        /// <summary>Gets <see cref="char"/> on the specified index.</summary>
        public char this[int index]
        {
            get => buffer[index + start];
            private set => buffer[index + start] = value;
        }

        /// <summary>Returns true if the buffer is empty.</summary>
        public bool IsEmpty() => Length == 0;
        
        /// <summary>Returns true if the buffer is not empty.</summary>
        public bool NotEmpty() => !IsEmpty();

        public bool IsUnknown(IFormatProvider provider) 
            => Unknown.IsUnknown(ToString(), provider as CultureInfo);

        public bool Matches(Regex regex) => regex.IsMatch(ToString());

        public char First() => buffer[start];

        public char Last() => buffer[end - 1];

        /// <summary>Gets the index of the <see cref="char"/> in the buffer.</summary>
        /// <returns>
        /// -1 if not found, otherwise the index of the <see cref="char"/>.
        /// </returns>
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
        public int LastIndexOf(char ch)
        {
            for (var i =end-1; i >= start; i--)
            {
                if (buffer[i] == ch)
                {
                    return i - start;
                }
            }
            return NotFound;
        }

        public bool EndOfBuffer(int index) => index >= Length;

        public bool StartsWith(string str)
        {
            if (str.Length > Length)
            {
                return false;
            }
            for (var i = 0; i < str.Length; i++)
            {
                if (this[i] != str[i])
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
            for (var i = start; i < end; i++)
            {
                if (buffer[i] == ch)
                {
                    count++;
                }
            }
            return count;
        }
   
      

        public CharBuffer Uppercase()
        {
            for (var i = start; i < end; i++)
            {
                buffer[i] = char.ToUpper(buffer[i], CultureInfo.InvariantCulture);
            }
            return this;
        }

        public CharBuffer Unify()
        =>  RemoveMarkup()
            .Uppercase()
            .ToNonDiacritic();

        public string Substring(int startIndex) => new string(buffer, startIndex + start, Length - startIndex);

        public string Substring(int startIndex, int length) => new string(buffer, startIndex + start, length);

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
                if (buffer[i + start] != other[i])
                {
                    if (ignoreCase && char.ToUpperInvariant(buffer[i + start]) == char.ToUpperInvariant(other[i]))
                    {
                        continue;
                    }
                    return false;
                }
            }
            return true;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
            => (obj is string str && Equals(str)) 
            || ReferenceEquals(this, obj);

        /// <inheritdoc />
        public override int GetHashCode() => throw new NotSupportedException();

        public static implicit operator string(CharBuffer buffer) => buffer?.ToString();

        public override string ToString() => new string(buffer, start, Length);

        private IEnumerable<char> Chars() => buffer.Skip(start).Take(Length);

        
    }
}
