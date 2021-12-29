namespace Qowaiv;

/// <summary>An implementation of <see cref="IComparer{T}"/> for <see cref="Guid"/> and <see cref="Uuid"/>.</summary>
public abstract class UuidComparer : IComparer<Uuid>, IComparer<Guid>, IComparer
{
    /// <summary>Gets the default comparer for <see cref="Guid"/> and <see cref="Uuid"/>.</summary>
    public static readonly UuidComparer Default = new UuidDefaultComparer();

    /// <summary>Gets the comparer for <see cref="Guid"/> and <see cref="Uuid"/> that used the MongoDB byte priority.</summary>
    public static readonly UuidComparer MongoDb = new UuidMongoDbComparer();

    /// <summary>Gets the comparer for <see cref="Guid"/> and <see cref="Uuid"/> that used the SQL Server byte priority.</summary>
    public static readonly UuidComparer SqlServer = new UuidSqlServerComparer();

    /// <summary>Gets the order of priority of the bytes of the <see cref="Guid"/>.</summary>
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
        var guidX = Cast(x);
        var guidY = Cast(y);

        if (guidX is null) return guidY is null ? 0 : -1;
        else if (guidY is null) return +1;
        else return Compare(guidX.Value, guidY.Value);

        static Guid? Cast(object? obj)
        {
            if (obj is null) return default;
            else if (obj is Guid guid) return guid;
            else if (obj is Uuid uuid) return uuid;
            else throw new NotSupportedException("Both arguments must be GUID/UUID.");
        }
    }
}
