using Qowaiv.Financial;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.ComponentModel.DataAnnotations.Financial
{
    /// <summary>Specifies the allowed <see cref="Qowaiv.Financial.Currency"/> for an <see cref="Money"/> field.</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class AllowedCurrenciesAttribute: ValidationAttribute
    {
        /// <summary>Creates a new instance of a <see cref="AllowedCurrenciesAttribute"/>.</summary>
        public AllowedCurrenciesAttribute(string value1, string value2)
            : this(new[] { value1, value2 }) { }

        /// <summary>Creates a new instance of a <see cref="AllowedCurrenciesAttribute"/>.</summary>
        /// <param name="values">
        /// String representations of the values.
        /// </param>
        /// <remarks>
        /// Also one string with a comma separated list is allowed.
        /// </remarks>
        public AllowedCurrenciesAttribute(params string[] values)
            : base(() => QowaivComponentModelMessages.AllowedCurrenciesAttribute_ValidationError)
        {
            Guard.HasAny(values, nameof(values));

            AllowedCurrencies = (values.Length == 1
                ? values[0].Split(',')
                : values)
                .Select(value => Currency.Parse(value?.Trim()))
                .ToArray();
        }

        /// <summary>Gets the values.</summary>
        public Currency[] AllowedCurrencies { get; }

        /// <summary>Returns true the value is null, or once the money has one of the allowed currencies.</summary>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            var money = Guard.IsTypeOf<Money>(value, nameof(value));
            return AllowedCurrencies.Any(allowed => money.Currency == allowed);
        }
    }
}
