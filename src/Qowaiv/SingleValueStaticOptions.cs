using System;

namespace Qowaiv
{
     /// <summary>Describes the available static options of a single value object.</summary>
    [Flags]
    public enum SingleValueStaticOptions
    {
        /// <summary>None.</summary>
        None = 0,
        /// <summary>The single value object has a static factory method T Parse(String).</summary>
        Parse = 1,
        /// <summary>The single value object has a static factory method bool TryParse(String, out T).</summary>
        TryParse = 2,
        /// <summary>The single value object has also (Try)Parse/IsValid factory methods with an extra CultureInfo paramater.</summary>
        CultureDependent = 4,
        /// <summary>The single value object is non-contious and the default value of it represents an empty value.</summary>
        HasEmptyValue = 8,
        /// <summary>The single value object is non-contious and the has a value that represents a not empty but unknown value.</summary>
        HasUnknownValue = 16,
        /// <summary>The single value object has an static bool IsValid(String) method, checking its potentional validness.</summary>
        IsValid = 32,

        /// <summary>The singe value object has static Parse/TryParse/IsValid methods, an empty value, an unknown and is culture depenedent.</summary>
        All = Parse | TryParse | CultureDependent | HasEmptyValue | HasUnknownValue | IsValid,

        /// <summary>The singe value object has static Parse/TryParse/IsValid methods, an empty value.</summary>
        AllExcludingCulture = All ^ CultureDependent,
    }
}
