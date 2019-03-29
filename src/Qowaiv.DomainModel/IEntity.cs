using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents an (domain-driven design) entity.</summary>
    /// <typeparam name="TEntity">
    /// The type of the actual entity.
    /// </typeparam>
    public interface IEntity<TEntity> : IEquatable<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        /// <summary>The identifier of the entity.</summary>
        [Key]
        Guid Id { get; }
    }
}
