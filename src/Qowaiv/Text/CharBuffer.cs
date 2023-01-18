#nullable enable

using System.Runtime.CompilerServices;

namespace Qowaiv.Text;

internal sealed partial class CharBuffer : IEnumerable<char>
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

    /// <summary>Gets the first <see cref="char"/> of the buffer.</summary>
    [Pure]
    public char First() => buffer[start];

    /// <summary>Gets the last <see cref="char"/> of the buffer.</summary>
    [Pure]
    public char Last() => buffer[end - 1];

    /// <inheritdoc />
    [Pure]
    public IEnumerator<char> GetEnumerator() => buffer.Skip(start).Take(Length).GetEnumerator();

    /// <inheritdoc />
    [Pure]
    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>Creates an empty buffer with the specified capacity.</summary>
    [Pure]
    public static CharBuffer Empty(int capacity) => new(capacity);
}
