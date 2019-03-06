namespace Qowaiv.DomainModel
{
    /// <summary><see cref="QowaivHash"/> is a helper class to construct hash
    /// codes based on multiple values.
    /// </summary>
    /// <remarks>
    /// Typical usage:
    /// 
    /// <code>
    /// public sealed class Address
    /// {
    ///     public override GetHashcode()
    ///     {
    ///         return QowaivHash.HashObject(Street)
    ///             ^ QowaivHash.Hash(HouseNumber, 3)
    ///             ^ QowaivHash.Hash(PostalCode, 5)
    ///             ^ QowaivHash.Hash(Country, 13)
    ///             ;
    ///     }
    /// }
    /// </code>
    /// </remarks>
    public static class QowaivHash
    {
        /// <summary>Gets the hash code of the structure.</summary>
        /// <param name="value">
        /// The structure to get an hash from.
        /// </param>
        /// <returns>
        /// A hash code.
        /// </returns>
        public static int Hash<T>(T value) where T : struct
        {
            return value.GetHashCode();
        }

        /// <summary>Gets the hash code of the structure.</summary>
        /// <param name="value">
        /// The structure to get an hash from.
        /// </param>
        /// <param name="shift">
        /// The number of positions the hash is shifted/rotated (should be between [0, 31]).
        /// </param>
        /// <returns>
        /// A hash code.
        /// </returns>
        public static int Hash<T>(T value, int shift) where T : struct
        {
            var hash = value.GetHashCode();
            return (hash << shift) | (hash >> (32 - shift));
        }

        /// <summary>Gets the hash code of the object.</summary>
        /// <param name="obj">
        /// The object to get an hash from.
        /// </param>
        /// <returns>
        /// Zero if the object was null, otherwise a hash code.
        /// </returns>
        public static int HashObject<T>(T obj) where T : class
        {
            return (obj is null) ? 0 : obj.GetHashCode();
        }

        /// <summary>Gets the hash code of the object.</summary>
        /// <param name="obj">
        /// The object to get an hash from.
        /// </param>
        /// <param name="shift">
        /// The number of positions the hash is shifted/rotated (should be between [0, 31]).
        /// </param>
        /// <returns>
        /// Zero if the object was null, otherwise a hash code.
        /// </returns>
        public static int HashObject<T>(T obj, int shift) where T : class
        {
            var hash = (obj is null) ? 0 : obj.GetHashCode();
            return (hash << shift) | (hash >> (32 - shift));
        }
    }
}
