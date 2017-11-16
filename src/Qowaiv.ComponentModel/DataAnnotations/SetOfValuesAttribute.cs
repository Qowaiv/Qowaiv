using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Base <see cref="ValidationAttribute"/> for allowing or forbidding a set of values.</summary>
    public abstract class SetOfValuesAttribute : ValidationAttribute
    {
        /// <summary>Creates a new instance of a <see cref="SetOfValuesAttribute"/>.</summary>
        protected SetOfValuesAttribute(string value1, string value2)
            : this(new[] { value1, value2 }) { }

        /// <summary>Creates a new instance of a <see cref="SetOfValuesAttribute"/>.</summary>
        /// <param name="values">
        /// String representations of the values.
        /// </param>
        protected SetOfValuesAttribute(params string[] values)
            : base(() => QowaivComponentModelMessages.AllowedValuesAttribute_ValidationError)
        {
            Values = Guard.NotNull(values, nameof(values));
        }

        /// <summary>The result to return when the value of <see cref="IsValid(object)"/>
        /// equals one of the values of the <see cref="SetOfValuesAttribute"/>.
        /// </summary>
        protected abstract bool OnEqual { get; }

        /// <summary>Gets the values.</summary>
        public string[] Values { get; }

        /// <summary>Returns true if the value occurs to be forbidden, otherwise false.</summary>
        public sealed override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            var converter = TypeDescriptor.GetConverter(value.GetType());
            return OnEqual == Values.Any(val => value.Equals(converter.ConvertFromString(val)));
        }
    }
}
