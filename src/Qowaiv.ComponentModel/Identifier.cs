using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Qowaiv.ComponentModel
{
    /// <summary>Factory class to retrieve the identifier (value) of a model.</summary>
    public static class Identifier
    {
        /// <summary>Gets the identifier value of the model.</summary>
        /// <param name="model">
        /// The model that should contain an identifier.
        /// </param>
        /// <returns>
        /// The value of the identifier.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When the model is null.
        /// </exception>
        /// <exception cref="IdentifierNotFoundException">
        /// When the model does not contain an identifier.
        /// </exception>
        public static object Get(object model)
        {
            if (TryGet(model, out object id))
            {
                return id;
            }
            throw new IdentifierNotFoundException();
        }

        /// <summary>Gets the identifier value of the model.</summary>
        /// <param name="model">
        /// The model that should contain an identifier.
        /// </param>
        /// <param name="id">
        /// THe value of the identifier.
        /// </param>
        /// <returns>
        /// The value of the identifier.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When the model is null.
        /// </exception>
        public static bool TryGet(object model, out object id)
        {
            Guard.NotNull(model, nameof(model));

            var type = model.GetType();

            // Not stored yet.
            if (!Store.TryGetValue(type, out PropertyInfo prop))
            {
                prop = ResolveId(type);
                if (prop is null)
                {
                    id = null;
                    return false;
                }
                else
                {
                    Store.TryAdd(type, prop);
                }
            }
            id = prop.GetValue(model);
            return true;
        }

        /// <summary>Resolves the ID of the data type.</summary>
        /// <remarks>
        /// Uses the <see cref="KeyAttribute"/> and the name convention 'ID' to resolve.
        /// </remarks>
        internal static PropertyInfo ResolveId(Type dataType)
        {
            var properties = dataType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var byKey = properties.FirstOrDefault(prop => prop.GetCustomAttributes<KeyAttribute>(true).Any());
            return byKey ?? properties.FirstOrDefault(IsIdentfier);
        }
        private static bool IsIdentfier(PropertyInfo property)
        {
            return "ID" == property.Name.ToUpperInvariant()
                && IdentifierTypes.Contains(property.PropertyType);
        }
        private static readonly Type[] IdentifierTypes = { typeof(Guid),typeof(Uuid), typeof(int), typeof(long) };

        private static readonly ConcurrentDictionary<Type, PropertyInfo> Store = new ConcurrentDictionary<Type, PropertyInfo>();
    }
}
