using Qowaiv.ComponentModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents a collection of entity properties.</summary>
    [Serializable]
    public class EntityPropertyCollection : Dictionary<string, EntityProperty>
    {
        /// <inheritdoc />
        private EntityPropertyCollection() { }

        /// <inheritdoc />
        protected EntityPropertyCollection(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>Returns true if any of the properties is dirty, otherwise false.</summary>
        public bool IsDirty => Values.Any(prop => prop.IsDirty);

        public static EntityPropertyCollection Create(Type type)
        {
            var annotated = AnnotatedModel.Create(type);

            var properties = new EntityPropertyCollection();

            foreach(var info in annotated.Properties)
            {
                if(!info.Descriptor.IsReadOnly)
                {
                    properties[info.Descriptor.Name] = new EntityProperty(info);
                }
            }

            return properties;
        }
    }
}
