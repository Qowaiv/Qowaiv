namespace Qowaiv.DomainModel.Collections;

/// <summary>Enumerator to iterate all elements in <see cref="ImmutableCollection"/>.</summary>
public struct Enumerator : IEnumerator<object>, IEnumerable<object>
{
    private readonly object[] Array;
    private readonly int End;
    private int Index;

    /// <summary>Initializes a new instance of the <see cref="Enumerator"/> struct.</summary>
    internal Enumerator(object[]? array, int start, int end)
    {
        Array = array ?? [];
        End = end;
        Index = start - 1;
    }

    /// <summary>Initializes a new instance of the <see cref="Enumerator"/> struct.</summary>
    internal Enumerator(object[] array, int count) : this(array, 0, count) { }

    /// <inheritdoc />
    public readonly object Current => Array[Index];

    /// <inheritdoc />
    [Pure]
    public bool MoveNext() => ++Index < End;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage/* Required for backward comparability. */]
    public void Reset() => throw new NotSupportedException();

    /// <inheritdoc />
    public readonly void Dispose() { /* Nothing to dispose. */ }

    /// <inheritdoc />
    [Pure]
    public readonly IEnumerator<object> GetEnumerator() => this;

    /// <inheritdoc />
    [Pure]
    [ExcludeFromCodeCoverage/* Required for backward comparability. */]
    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
