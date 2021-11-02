using Qowaiv.Hashing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Qowaiv.Text
{
    internal sealed partial class CharBuffer : IEquatable<string>, IEnumerable<char>
    {
        internal static readonly int NotFound = -1;

        private readonly char[] buffer;
        private int start;
        private int end;

        /// <summary>Initializes a new instance of the <see cref="CharBuffer"/> class.</summary>
        private CharBuffer(int capacity) => buffer = new char[capacity];

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
        [Pure]
        public bool IsEmpty() => Length == 0;

        /// <summary>Returns true if the buffer is not empty.</summary>
        [Pure]
        public bool NotEmpty() => !IsEmpty();

        /// <summary>Returns true if the buffer represents an unknown value.</summary>
        [Pure]
        public bool IsUnknown(IFormatProvider provider) 
            => Unknown.IsUnknown(ToString(), provider as CultureInfo);

        /// <summary>Returns true if the buffer matches the specified <see cref="Regex"/>.</summary>
        [Pure]
        public bool Matches(Regex regex) => regex.IsMatch(ToString());

        /// <summary>Gets the first <see cref="char"/> of the buffer.</summary>
        [Pure]
        public char First() => buffer[start];

        /// <summary>Gets the last <see cref="char"/> of the buffer.</summary>
        [Pure]
        public char Last() => buffer[end - 1];

        /// <summary>Returns true if index is the end of the buffer.</summary>
        [Pure]
        public bool EndOfBuffer(int index) => index >= Length - 1;

        /// <inheritdoc />
        [Pure]
        public bool Equals(string other) => Equals(other, false);

        /// <summary>Returns true if the buffer equals the <see cref="string"/>.</summary>
        [Pure]
        public bool Equals(string other, bool ignoreCase)
        {
            if (Length != other.Length)
            {
                return false;
            }
            for (var i = 0; i < Length; i++)
            {
                if (this[i] != other[i])
                {
                    if (ignoreCase && char.ToUpperInvariant(this[i]) == char.ToUpperInvariant(other[i]))
                    {
                        continue;
                    }
                    return false;
                }
            }
            return true;
        }

        /// <inheritdoc />
        [Pure]
        public override bool Equals(object obj)
            => (obj is string str && Equals(str)) 
            || ReferenceEquals(this, obj);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => Hash.NotSupportedBy<CharBuffer>();

        /// <inheritdoc />
        [Pure]
        public IEnumerator<char> GetEnumerator() => Enumerate().GetEnumerator();

        /// <inheritdoc />
        [Pure]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>Enumerates through all (visible) chars of the buffer.</summary>
        [Pure]
        private IEnumerable<char> Enumerate() => buffer.Skip(start).Take(Length);

        /// <summary>Creates an empty buffer with the specified capacity.</summary>
        [Pure]
        public static CharBuffer Empty(int capacity) => new(capacity);
    }
}
