namespace Qowaiv.Text;

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
    public bool StartsWithCaseInsensitve(string str)
    {
        if (str.Length > Length) return false;
        else
        {
            for (var i = 0; i < str.Length; i++)
            {
                if (char.ToUpperInvariant(this[i]) != char.ToUpperInvariant(str[i])) { return false; }
            }
            return true;
        }
    }
      
    /// <summary>Retrieves a substring from the buffer..</summary>
    [Pure]
    public string Substring(int startIndex) => new(buffer, startIndex + start, Length - startIndex);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => new(buffer, start, Length);

    /// <summary>Implicitly casts a buffer to a <see cref="string"/>.</summary>
    public static implicit operator string(CharBuffer buffer) => buffer.ToString();
}
