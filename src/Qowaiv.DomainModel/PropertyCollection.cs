using Qowaiv.ComponentModel.Validation;
using Qowaiv.DomainModel.Persistence;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
                prop.Value.Init(prop.Value.Annotations.DefaultValue);
            }
        }

        /// <summary>Returns true if any of the properties is dirty, otherwise false.</summary>
        public bool IsDirty => Values.Any(prop => prop.IsDirty);

        /// <summary>Gets the delta of the properties.</summary>
        public Delta GetDelta()
        {
            var delta = new Delta();

            foreach (var kvp in this.Where(item => item.Value.IsDirty))
            {
                delta.New[kvp.Key] = kvp.Value.Value;
                delta.Old[kvp.Key] = kvp.Value.Initial;
            }
            return delta;
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
