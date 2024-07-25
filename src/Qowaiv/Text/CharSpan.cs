namespace Qowaiv.Text;

internal readonly struct CharSpan : IEquatable<CharSpan>, IEquatable<string>, IEnumerable<char>
{
    public static readonly CharSpan Empty = new(string.Empty, 0, 0);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string m_Value;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly int Start;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly int End;

    public CharSpan(string str) : this(str, 0, str.Length) { }

    private CharSpan(string str, int start, int end)
    {
        m_Value = str;
        Start = start;
        End = end;
    }

    /// <summary>Gets the number of characters in the char span.</summary>
    public int Length => End - Start;

    /// <summary>Gets the char on the specific index.</summary>
    public char this[int index] => m_Value[Start + index];

    public char First => m_Value[Start];

    public bool IsEmpty => Length <= 0;

    [Pure]
    public bool NotEmpty => Length > 0;

    [Pure]
    public bool StartsWith(string value, bool ignoreCase = false)
    {
        if (value.Length > Length) return false;
        var start = Start;

        return ignoreCase
            ? CaseInsensitive(start, m_Value, value)
            : CaseSensitive(start, m_Value, value);

        static bool CaseInsensitive(int start, string value,  string startsWith)
        {
            for (var i = 0; i < startsWith.Length; i++)
            {
                if (char.ToUpperInvariant(value[start++]) != char.ToUpperInvariant(startsWith[i]))
                {
                    return false;
                }
            }
            return true;
        }

        static bool CaseSensitive(int start, string value, string startsWith)
        {
            for (var i = 0; i < startsWith.Length; i++)
            {
                if (value[start++] != startsWith[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

    [Pure]
    public CharSpan Prev(int steps) => new(m_Value, Start - steps, End);

    [Pure]
    public CharSpan Next() => new(m_Value, Start + 1, End);

    [Pure]
    public CharSpan Next(int steps) => new(m_Value, Start + steps, End);

    [Pure]
    public CharSpan Last(out char ch)
    {
        ch = m_Value[End - 1];
        return new(m_Value, Start, End - 1);
    }

    [Pure]
    public CharSpan TrimLeft(Func<char, bool> predicate, out CharSpan trimmed)
    {
        for (var index = Start; index < End; index++)
        {
            if (!predicate(m_Value[index]))
            {
                trimmed = new(m_Value, Start, index);
                return new(m_Value, index, End);
            }
        }
        trimmed = this;
        return Empty;
    }

    [Pure]
    public CharSpan TrimRight() => TrimRight(char.IsWhiteSpace, out _);

    [Pure]
    public CharSpan TrimRight(Func<char, bool> predicate, out CharSpan trimmed)
    {
        for (var index = End - 1; index >= Start; index--)
        {
            if (!predicate(m_Value[index]))
            {
                trimmed = new(m_Value, index + 1, End);
                return new(m_Value, Start, index + 1);
            }
        }
        trimmed = this;
        return Empty;
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => m_Value is null ? string.Empty : m_Value.Substring(Start, Length);

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is CharSpan other && Equals(other);

    [Pure]
    public bool Equals(CharSpan other)
    {
        if (Length == other.Length)
        {
            for (var i = 0; i < Length; i++)
            {
                if (this[i] != other[i]) return false;
            }
            return true;
        }
        else return false;
    }

    [Pure]
    public bool Equals(string? other) => Equals(other.CharSpan());

    [Pure]
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => Hash.Code(ToString());

    [Pure]
    public IEnumerator<char> GetEnumerator() => m_Value.Skip(Start).Take(Length).GetEnumerator();

    [Pure]
    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static CharSpan operator ++(CharSpan span) => span.Next();
}
