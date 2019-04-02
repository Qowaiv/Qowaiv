using Qowaiv.ComponentModel.Validation;
using System;
using System.Collections.Generic;
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
            var annotated = AnnotatedModel.Get(type);

            var properties = new PropertyCollection(annotated.Properties.Count);

            foreach (var info in annotated.Properties)
            {
                if(info.IsReadOnly)
                {
                    continue;
                }
                properties[info.Name] = info.DefaultValue;
            }
            return properties;
        }
    }
}
