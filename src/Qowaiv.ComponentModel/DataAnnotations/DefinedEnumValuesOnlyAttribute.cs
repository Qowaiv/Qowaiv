using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Validates if the decorated enum has a value that is a defined.</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DefinedEnumValuesOnlyAttribute : ValidationAttribute
    {
        /// <summary>Creates a new instance of a <see cref="DefinedEnumValuesOnlyAttribute"/>.</summary>
        public DefinedEnumValuesOnlyAttribute()
        : base(() => QowaivComponentModelMessages.AllowedValuesAttribute_ValidationError) { }

        /// <summary>If true, for flag enums, also combinations of defined single values are allowed, that are not defined themselves explicitly.</summary>
        public bool AllowUndefinedFlagCombinations { get; set; } = true;

        /// <summary>Returns true if the value is defined for the enum, otherwise false.</summary>
        /// <exception cref="ArgumentException">
        /// If the type of the value is not an enum.
        /// </exception>
        public override bool IsValid(object value)
        {
            // Might be a nullable enum, we just don't know.
            if(value is null)
            {
                return true;
            }

            var enumType = value.GetType();

            if(AllowUndefinedFlagCombinations && enumType.IsEnum && enumType.GetCustomAttributes<FlagsAttribute>().Any())
            {
                dynamic dyn = value;
                var max = Enum.GetValues(enumType)
                    .Cast<dynamic>()
                    .Aggregate((e1, e2) => e1 | e2);

                return (max & dyn) == dyn;
            }


            return Enum.IsDefined(enumType, value);
        }
    }
}
