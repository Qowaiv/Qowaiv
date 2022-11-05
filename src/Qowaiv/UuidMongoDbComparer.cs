namespace Qowaiv;

/// <summary>Implements the <see cref="UuidComparer"/> for SQL Server.</summary>
internal sealed class UuidMongoDbComparer : UuidComparer
{
    /// <inheritdoc/>
    public override IReadOnlyList<int> Priority { get; } = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

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
