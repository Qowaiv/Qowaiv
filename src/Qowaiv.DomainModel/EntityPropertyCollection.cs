using Qowaiv.ComponentModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qowaiv.DomainModel
{
    public class EntityPropertyCollection : Dictionary<string, EntityProperty>
    {
        private EntityPropertyCollection() { }

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
