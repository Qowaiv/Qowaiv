using System;
using System.Collections.Generic;

namespace Qowaiv.DomainModel
{
    /// <summary>Compares entities.</summary>
    public class EntityEqualityComparer<TEntity, TId> : IEqualityComparer<TEntity>
        where TEntity : class, IEntity<TEntity, TId>
        where TId : struct
    {
        /// <summary>Returns true if both entities have the same type,
        /// are not transient, and have the same id, otherwise false.
        /// </summary>
        public bool Equals(TEntity x, TEntity y)
        {
            if (x is null || y is null)
            {
                return ReferenceEquals(x, y);
            }

            return
                x.GetType().Equals(y.GetType()) &&
                x.Id.Equals(y.Id) &&
                !default(TId).Equals(x.Id) &&
                !default(TId).Equals(y.Id);
        }

        /// <summary>Gets a hash code for the entity.</summary>
        /// <exception cref="ArgumentNullException">
        /// If the entity is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// When an entity is transient, as the id (which is the source of the hash)
        /// is likely to change.
        /// </exception>
        public int GetHashCode(TEntity obj)
        {
            Guard.NotNull(obj, nameof(obj));

            return default(TId).Equals(obj.Id)
                ? throw new NotSupportedException(QowaivDomainModelMessages.NotSupported_GetHashCodeOnIsTransient)
                : obj.Id.GetHashCode() ^ obj.GetType().GetHashCode();
        }
    }
}
