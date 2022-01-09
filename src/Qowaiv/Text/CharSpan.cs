namespace Qowaiv.Text
{

    internal readonly struct CharSpan : IEquatable<CharSpan>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string m_Value;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int Start;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int End;

        public CharSpan(string str) : this(str, 0, str.Length - 1) { }

        private CharSpan(string str, int start, int end)
        {
            m_Value = str;
            Start = start;
            End = end;
        }

        /// <summary>Gets the number of characters in the char span.</summary>
        public int Length => End - Start + 1;

        /// <summary>Gets the char on the specific index.</summary>
        public char this[int index] => m_Value[Start + index];

        [Pure]
        public bool IsEmpty() => Length <= 0;

        [Pure]
        public bool NotEmpty() => Length > 0;

        [Pure]
        public char First() => m_Value[Start];

        [Pure]
        public char Last() => m_Value[End];

        [Pure]
        public CharSpan TrimLeft() => TrimLeft(char.IsWhiteSpace, out _);

        [Pure]
        public CharSpan TrimLeft(Func<char, bool> predicate, out CharSpan trimmed)
        {
            for(var index = Start; index <= End; index++)
            {
                if (!predicate(m_Value[index]))
                {
                    trimmed = new(m_Value, Start, index - 1);
                    return new(m_Value, index, End);
                }
            }
            trimmed = default;
            return this;
        }

        [Pure]
        public CharSpan TrimRight() => TrimRight(char.IsWhiteSpace, out _);

        [Pure]
        public CharSpan TrimRight(Func<char, bool> predicate, out CharSpan trimmed)
        {
            for (var index = End; index >= Start; index--)
            {
                if (!predicate(m_Value[index]))
                {
                    trimmed = new(m_Value, index + 1, End);
                    return new(m_Value, Start, index);
                }
            }
            trimmed = default;
            return this;
        }

        /// <inheritdoc />
        [Pure]
        public override string ToString() => m_Value is null ? string.Empty : m_Value.Substring(Start, Length);

        /// <inheritdoc />
        [Pure]
        public override bool Equals(object obj) => obj is CharSpan other && Equals(other);

        [Pure]
        public bool Equals(CharSpan other)
        {
            if(Length == other.Length)
            {
                for(var i = 0; i < Length; i++)
                {
                    if(this[i] != other[i]) return false;
                }
                return true;
            }
            else return false;
        }

        [Pure]
        public override int GetHashCode() => Hash.Code(ToString());
    }
}
