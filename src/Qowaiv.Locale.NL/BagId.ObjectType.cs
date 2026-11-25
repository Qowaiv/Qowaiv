using System.Collections.Frozen;

namespace Qowaiv.Locale.NL;

public readonly partial struct BagId
{
    /// <summary>TODO.</summary>
    public enum ObjectType
    {
        /// <summary>None/empty.</summary>
        None = 00,

        /// <summary>Veblijfsobject (Translation).</summary>
        Verblijfsobject /*..*/ = 01,

        /// <summary>Ligplaats (Translation).</summary>
        Ligplaats /*........*/ = 02,

        /// <summary>Standplaats (Translation).</summary>
        Standplaats /*......*/ = 03,

        /// <summary>Pand (Translation).</summary>
        Pand /*.............*/ = 10,

        /// <summary>Nummeraanduiding (Translation).</summary>
        Nummeraanduiding /*.*/ = 20,

        /// <summary>Unknown.</summary>
        Unknown = 99,
    }

    [Pure]
    private static bool IsValidObject(ObjectType code) => code
        is ObjectType.Verblijfsobject
        or ObjectType.Ligplaats
        or ObjectType.Standplaats
        or ObjectType.Pand
        or ObjectType.Nummeraanduiding;

}
