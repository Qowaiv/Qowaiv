using System;
using System.Collections;
using System.Collections.Generic;

namespace Qowaiv
{
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
        public abstract int Compare(Guid x, Guid y);

        /// <inheritdoc/>
        public int Compare(Uuid x, Uuid y) => Compare((Guid)x, (Guid)y);

        /// <inheritdoc/>
        public int Compare(object x, object y)
        {
            if (x is null)
            {
                if (y is null)
                {
                    return 0;
                }
                if (y is Guid || y is Uuid)
                {
                    return -1;
                }
            }
            if (x is Guid || x is Uuid)
            {
                if (y is null)
                {
                    return +1;
                }
                if (y is Guid || y is Uuid)
                {
                    var a = x is Guid g ? g : (Guid)(Uuid)x;
                    var b = y is Guid h ? h : (Guid)(Uuid)y;
                    return Compare(a, b);
                }
            }
            throw new ArgumentException($"Both arguments must be GUID/UUID.");
        }
    }
}
