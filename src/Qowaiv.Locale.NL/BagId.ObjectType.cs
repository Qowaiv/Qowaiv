#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

namespace Qowaiv.Locale.NL;

public readonly partial struct BagId
{
    /// <summary>The type of BAG object identified by the number.</summary>
    public enum ObjectType
    {
        /// <summary>None/empty.</summary>
        None = 00,

        /// <summary>Verblijfsobject (residential object).</summary>
        Verblijfsobject = 01,

        /// <summary>Ligplaats (mooring).</summary>
        Ligplaats = 02,

        /// <summary>Standplaats (pitch).</summary>
        Standplaats = 03,

        /// <summary>Pand (building).</summary>
        Pand = 10,

        /// <summary>Nummeraanduiding (address designation).</summary>
        Nummeraanduiding = 20,

        /// <summary>Openbare ruimte (public space).</summary>
        OpenbareRuimte = 30,

        /// <summary>Unknown.</summary>
        Unknown = 99,
    }

    [Pure]
    private static bool IsValidObject(ObjectType code) => code
        is ObjectType.Verblijfsobject
        or ObjectType.Ligplaats
        or ObjectType.Standplaats
        or ObjectType.Pand
        or ObjectType.Nummeraanduiding
        or ObjectType.OpenbareRuimte;
}
