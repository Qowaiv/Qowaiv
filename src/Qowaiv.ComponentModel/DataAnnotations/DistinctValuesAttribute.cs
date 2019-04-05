using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Specifies that all values are distinct.</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DistinctValuesAttribute : ValidationAttribute
    {
        /// <summary>Creates a new instance of <see cref="DistinctValuesAttribute"/>.</summary>
        public DistinctValuesAttribute(Type comparer = null)
            : base(() => QowaivComponentModelMessages.DistinctValuesAttribute_ValidationError)
        {
            EqualityComparer = CreateComparer(comparer);
        }

        /// <summary>Gets and set a custom <see cref="IEqualityComparer"/>.</summary>
        public IEqualityComparer EqualityComparer { get; }

        /// <summary>True if all items in the collection are distinct, otherwise false.</summary>
        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            var collection = Guard.IsTypeOf<IEnumerable>(value, nameof(value));
            var capacity = collection is ICollection c ? c.Count : 4;

            var count = 0;
            var checker = new Hashtable(capacity, EqualityComparer);

            foreach (var item in collection)
            {
                // Only add the key. We don't care about the value.
                checker.Add(item, null);

                //  If adding did not increase the size of the checker, we're done.
                if (checker.Count == count++)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>Creates the Comparer to do the distinct with.</summary>
        private static IEqualityComparer CreateComparer(Type comparer)
        {
            return comparer is null
                ? EqualityComparer<object>.Default
                : Guard.IsTypeOf<IEqualityComparer>(Activator.CreateInstance(comparer), nameof(comparer));
        }
    }
}
