using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Specifies that a field should at least have one item in its collection.</summary>
    public class AnyAttribute : RequiredAttribute
    {
        /// <summary>Returns true if the value is not null and the collection
        /// has any item, otherwise false.
        /// </summary>
        public override bool IsValid(object value)
        {
            if (value is IEnumerable enumerable)
            {
                return enumerable.GetEnumerator().MoveNext();
            }
            return base.IsValid(value);
        }
    }
}
