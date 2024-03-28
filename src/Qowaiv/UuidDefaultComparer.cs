namespace Qowaiv;

/// <summary>The default implementation of the <see cref="UuidComparer"/>.</summary>
internal sealed class UuidDefaultComparer : UuidComparer
{
    /// <inheritdoc/>
    /// <remarks>
    /// Internally, a <see cref="Guid"/>  is built up with:
    /// 1 <see cref="uint"/>
    /// 2 <see cref="ushort"/>
    /// 8 <see cref="byte"/>.
    /// </remarks>
    public override IReadOnlyList<int> Priority { get; } = [3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15];

    /// <inheritdoc/>
    [Pure]
    public override int Compare(Guid x, Guid y) => x.CompareTo(y);
}
