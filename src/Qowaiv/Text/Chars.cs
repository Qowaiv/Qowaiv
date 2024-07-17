namespace Qowaiv.Text;

/// <summary>Lightweight <see cref="char"/> buffer.</summary>
/// <remarks>
/// Keep in mind that the char array is shared.
/// </remarks>
internal readonly struct Chars(char[] value, int length)
{
    private readonly char[] m_Value = value;

    public readonly int Length = length;

    public char Last => Length == 0 ? default : m_Value[Length - 1];

    [Pure]
    private Chars Add(char c)
    {
        var l = Length;
        m_Value[l++] = c;
        return new(m_Value, l);
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => new(m_Value, 0, Length);

    public static Chars operator +(Chars chars, char ch) => chars.Add(ch);

    [Pure]
    public static Chars Init(int capacity) => new(new char[capacity], 0);
}
