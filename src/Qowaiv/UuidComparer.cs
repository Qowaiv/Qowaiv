namespace Qowaiv;

/// <summary>An implementation of <see cref="IComparer{T}" /> for <see cref="Guid" /> and <see cref="Uuid" />.</summary>
public abstract class UuidComparer : IComparer<Uuid>, IComparer<Guid>, IComparer
{
    /// <summary>Gets the default comparer for <see cref="Guid" /> and <see cref="Uuid" />.</summary>
    public static readonly UuidComparer Default = new UuidDefaultComparer();

    /// <summary>Gets the comparer for <see cref="Guid" /> and <see cref="Uuid" /> that used the MongoDB byte priority.</summary>
    public static readonly UuidComparer MongoDb = new UuidMongoDbComparer();

    /// <summary>Gets the comparer for <see cref="Guid" /> and <see cref="Uuid" /> that used the SQL Server byte priority.</summary>
    public static readonly UuidComparer SqlServer = new UuidSqlServerComparer();

    /// <summary>Gets the order of priority of the bytes of the <see cref="Guid" />.</summary>
    public abstract IReadOnlyList<int> Priority { get; }

    /// <inheritdoc/>
    [Pure]
    public abstract int Compare(Guid x, Guid y);

    /// <inheritdoc/>
    [Pure]
    public int Compare(Uuid x, Uuid y) => Compare((Guid)x, (Guid)y);

    /// <inheritdoc/>
    [Pure]
    public int Compare(object? x, object? y)
    {
        return (Cast(x), Cast(y)) switch
        {
            ({ }, null) => +1,
            (null, { }) => -1,
            (Guid gx, Guid gy) => Compare(gx, gy),
            _ => 0,
        };

        static Guid? Cast(object? obj) => obj switch
        {
            null => null,
            Guid guid => guid,
            Uuid uuid => (Guid)uuid,
            _ => throw new NotSupportedException("Both arguments must be GUID/UUID."),
        };
    }
}
