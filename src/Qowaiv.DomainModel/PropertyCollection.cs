﻿using Qowaiv.ComponentModel.Validation;
using System.Collections.Generic;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents a collection of entity properties.</summary>
    internal class PropertyCollection : Dictionary<string, object>
    {
        /// <summary>No public constructor</summary>
        private PropertyCollection(int capacity) : base(capacity) { }
        
        /// <summary>Creates the properties for the type.</summary>
        public static PropertyCollection Create<TEntity>(TEntity entity) where TEntity : class
        {
            Guard.NotNull(entity, nameof(entity));

            var annotated = AnnotatedModel.Get(entity.GetType());

            var properties = new PropertyCollection(annotated.Properties.Count);

            foreach (var info in annotated.Properties)
            {
                if(info.Descriptor.IsReadOnly)
                {
                    continue;
                }
                properties[info.Descriptor.Name] = info.DefaultValue;
            }
            return properties;
        }
    }
}
