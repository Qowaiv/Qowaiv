using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Specifies that all values are distinct.</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DistinctValuesAttribute : ValidationAttribute
    {
        /// <summary>Creates a new instance of <see cref="DistinctValuesAttribute"/>.</summary>
        /// <remarks>
        /// The type of the custom <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/> (for <see cref="object"/>).
        /// </remarks>
        public DistinctValuesAttribute(Type comparer = null)
            : base(() => QowaivComponentModelMessages.DistinctValuesAttribute_ValidationError)
        {
            EqualityComparer = CreateComparer(comparer);
        }

        /// <summary>Gets and set a custom <see cref="IEqualityComparer"/>.</summary>
        public IEqualityComparer<object> EqualityComparer { get; }

        /// <summary>True if all items in the collection are distinct, otherwise false.</summary>
        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            var collection = Guard.IsInstanceOf<IEnumerable>(value, nameof(value)).Cast<object>();
            var checker = new HashSet<object>(EqualityComparer);

            return collection.All(checker.Add);
        }

        /// <summary>Creates the Comparer to do the distinct with.</summary>
        private static IEqualityComparer<object> CreateComparer(Type comparer)
        {
            if (comparer is null)
            {
                return EqualityComparer<object>.Default;
            }
            if(typeof(IEqualityComparer<object>).IsAssignableFrom(comparer))
            {
                return (IEqualityComparer<object>)Activator.CreateInstance(comparer);
            }
            if (typeof(IEqualityComparer).IsAssignableFrom(comparer))
            {
                return new WrappedComparer((IEqualityComparer)Activator.CreateInstance(comparer));
            }
            throw new ArgumentException(string.Format(QowaivComponentModelMessages.ArgumentException_TypeIsNotEqualityComparer, comparer), nameof(comparer));
        }

        /// <summary>As there is no none generic hash set.</summary>
        private class WrappedComparer : IEqualityComparer<object>
        {
            private readonly IEqualityComparer _comparer;
            public WrappedComparer(IEqualityComparer comparer) => _comparer = comparer;
            public new bool Equals(object x, object y) => _comparer.Equals(x, y);
            public int GetHashCode(object obj) => _comparer.GetHashCode(obj);
        }
    }
}
