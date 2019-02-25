using Qowaiv.ComponentModel.Validation;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents a collection of entity properties.</summary>
    public class PropertyCollection : ReadOnlyDictionary<string, Property>
    {
        /// <summary>No public constructor</summary>
        private PropertyCollection(Dictionary<string, Property> properties) : base(properties)
        {
            foreach (var prop in this)
            {
                prop.Value.SetOnly(prop.Value.Annotations.DefaultValue);
            }
        }

        /// <summary>Creates the properties for the type.</summary>
        public static PropertyCollection Create<TId>(IEntity<TId> entity) where TId : struct
        {
            Guard.NotNull(entity, nameof(entity));

            var annotated = AnnotatedModel.Create(entity.GetType());

            var properties = new Dictionary<string, Property>();

            foreach (var info in annotated.Properties)
            {
                if (!info.Descriptor.IsReadOnly)
                {
                    var property = new Property(info, entity);

                    properties[info.Descriptor.Name] = property;
                }
            }
            return new PropertyCollection(properties);
        }
    }
}
