using Qowaiv.ComponentModel.Validation;
using Qowaiv.DomainModel.DataAnnotations;
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
            foreach (var prop in GetEditiable())
            {
                prop.Value = prop.Annotations.DefaultValue;
            }
        }

        /// <summary>Gets the editable properties.</summary>
        private IEnumerable<Property> GetEditiable() => Values.Where(p => !(p is CalculatedProperty));

        /// <summary>Gets the calculated properties.</summary>
        private IEnumerable<CalculatedProperty> GetCalculated() => Values.Where(p => p is CalculatedProperty).Cast<CalculatedProperty>();

        /// <summary>Creates the properties for the type.</summary>
        public static PropertyCollection Create<TId>(IEntity<TId> entity) where TId : struct
        {
            Guard.NotNull(entity, nameof(entity));

            var annotated = AnnotatedModelStore.Instance.GetAnnotededModel(entity.GetType());

            var properties = new Dictionary<string, Property>();

            foreach (var info in annotated.Properties)
            {
                if(SkipProperty(info.Descriptor.Name))
                {
                    continue;
                }

                var property = info.Descriptor.IsReadOnly 
                    ? new CalculatedProperty(info, entity)
                    : new Property(info, entity);

                properties[info.Descriptor.Name] = property;
            }
            var collection = new PropertyCollection(properties);

            foreach(var calculated in collection.GetCalculated())
            {
                var dependsOn = (DependsOnAttribute)calculated.Annotations.Descriptor.Attributes[typeof(DependsOnAttribute)];
                if(dependsOn != null)
                {
                    foreach(var property in dependsOn.DependingProperties)
                    {
                        collection[property].TriggersProperties.Add(calculated);
                    }
                }
            }
            return collection;
        }

        private static bool SkipProperty(string name)
        {
            return nameof(Entity<int>.IsTransient) == name
                || nameof(Entity<int>.Properties) == name;
        }
    }
}
