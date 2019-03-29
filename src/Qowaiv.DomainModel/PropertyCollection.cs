using Qowaiv.ComponentModel.Validation;
using System;
using System.Collections.Generic;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents a collection of entity properties.</summary>
    internal class PropertyCollection : Dictionary<string, object>
    {
        /// <summary>No public constructor</summary>
        private PropertyCollection(int capacity) : base(capacity) { }
        
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
