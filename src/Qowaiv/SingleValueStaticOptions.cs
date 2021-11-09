namespace Qowaiv;

/// <summary>Describes the available static options of a single value object.</summary>
[Flags]
public enum SingleValueStaticOptions
{
    /// <summary>None.</summary>
    None = 0,
    /// <summary>The single value object has a static factory method T Parse(String).</summary>
    Parse = 1,
    /// <summary>The single value object has a static factory method Boolean TryParse(String, out T).</summary>
    TryParse = 2,
    /// <summary>The single value object has also (Try)Parse/IsValid factory methods with an extra CultureInfo parameter.</summary>
    CultureDependent = 4,
    /// <summary>The single value object is non-continuous and the default value of it represents an empty value.</summary>
    HasEmptyValue = 8,
    /// <summary>The single value object is non-continuous and the has a value that represents a not empty but unknown value.</summary>
    HasUnknownValue = 16,
    /// <summary>The single value object has an static Boolean IsValid(String) method, checking its positional validness.</summary>
    IsValid = 32,

    /// <summary>The singe value object has static Parse/TryParse/IsValid methods, a MinValue and MaxVlaue constant, and is culture dependent.</summary>
    Continuous = Parse | TryParse | CultureDependent | IsValid,

    /// <summary>The singe value object has static Parse/TryParse/IsValid methods, an empty value, an unknown and is culture dependent.</summary>
    All = Parse | TryParse | CultureDependent | HasEmptyValue | HasUnknownValue | IsValid,

    /// <summary>The singe value object has static Parse/TryParse/IsValid methods, an empty value.</summary>
    AllExcludingCulture = All ^ CultureDependent,
}
