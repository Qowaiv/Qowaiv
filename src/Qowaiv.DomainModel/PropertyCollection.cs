using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents a collection of entity properties.</summary>
    [Serializable]
    public class PropertyCollection : Dictionary<string, object>
    {
        /// <summary>No public constructor</summary>
        protected PropertyCollection(int capacity) : base(capacity) { }

        /// <summary>Initializes a new instance of a <see cref="PropertyCollection"/> with serialized data.</summary>
        protected PropertyCollection(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>Creates the properties for the type.</summary>
        public static PropertyCollection Create(Type type)
        {
            Guard.NotNull(type, nameof(type));

            var descriptors = TypeDescriptor.GetProperties(type)
                .Cast<PropertyDescriptor>()
                .Where(desc => !desc.IsReadOnly)
                .ToArray();

            var properties = new PropertyCollection(descriptors.Length);

            foreach (var desc in descriptors)
            {
                properties[desc.Name] = desc.PropertyType.IsValueType ? Activator.CreateInstance(desc.PropertyType) : null;
            }
            return properties;
        }
    }
}
