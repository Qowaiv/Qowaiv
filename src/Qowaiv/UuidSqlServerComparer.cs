namespace Qowaiv;

/// <summary>Implements the <see cref="UuidComparer" /> for SQL Server.</summary>
internal sealed class UuidSqlServerComparer : UuidComparer
{
    /// <inheritdoc/>
    /// <remarks>
    /// E.a.w:
    /// 10: the most significant byte in Guid ByteArray [for SQL Server ORDERY BY clause]
    ///  3: the least significant byte in Guid ByteArray [for SQL Server ORDER BY clause].
    /// </remarks>
    public override IReadOnlyList<int> Priority { get; } = [10, 11, 12, 13, 14, 15, 8, 9, 6, 7, 4, 5, 0, 1, 2, 3];

    /// <inheritdoc/>
    [Pure]
    public override int Compare(Guid x, Guid y)
    {
        var a = x.ToByteArray();
        var b = y.ToByteArray();

        var compare = 0;

        foreach (var index in Priority)
        {
            compare = a[index].CompareTo(b[index]);
            if (compare != 0)
            {
                return compare;
            }
        }
        return compare;
    }
}
