using System;
using System.Collections.Generic;

namespace Qowaiv.DomainModel
{
    /// <summary>Compares entities.</summary>
    public class EntityEqualityComparer<TId> : IEqualityComparer<IEntity<TId>>
        where TId : struct
    {
        /// <inheritdoc />
        public bool Equals(IEntity<TId> x, IEntity<TId> y)
        {
            if (x is null || y is null)
            {
                return ReferenceEquals(x, y);
            }

            return
                x.GetType().Equals(y.GetType()) &&
                x.Id.Equals(y.Id) &&
                !x.IsTransient() &&
                !y.IsTransient();
        }

        /// <inheritdoc />
        public int GetHashCode(IEntity<TId> obj)
        {
            Guard.NotNull(obj, nameof(obj));

            return obj.IsTransient()
                ? throw new NotSupportedException(QowaivDomainModelMessages.NotSupported_GetHashCodeOnIsTransient)
                : obj.Id.GetHashCode() ^ obj.GetType().GetHashCode();
        }
    }
}
