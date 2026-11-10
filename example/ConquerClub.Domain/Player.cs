#pragma warning disable S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"
// Players should be sortable, but less than, or greater than has no meaning here.
using Qowaiv;

namespace ConquerClub.Domain;

/// <summary>Represents a player identifier.</summary>
public readonly struct Player(byte id) : IEquatable<Player>, IComparable<Player>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly byte Id = id;

    /// <summary>Represents the neutral <see cref="Player"/>.</summary>
    public static readonly Player Neutral;

    /// <summary>Represents an unknown <see cref="Player"/>.</summary>
    public static readonly Player Unknown = new(byte.MaxValue);

    /// <summary>Gets player P1.</summary>
    public static readonly Player P1 = new(1);

    /// <summary>Gets player P2.</summary>
    public static readonly Player P2 = new(2);

    /// <summary>Gets player P3.</summary>
    public static readonly Player P3 = new(3);

    /// <summary>Creates an army for the player.</summary>
    [Pure]
    public Army Army(int size) 
        => size == 0
        ? Domain.Army.None
        : new(this, Guard.Positive(size));

    [Pure]
    public bool IsOther(Player other) => !Equals(other);

    /// <inheritdoc/>
    [Pure]
    public override bool Equals(object? obj) => obj is Player other && Equals(other);

    /// <inheritdoc/>
    [Pure] 
    public bool Equals(Player other) => Id == other.Id;

    /// <inheritdoc/>
    [Pure] 
    public int CompareTo(Player other) => Id.CompareTo(other.Id);

    /// <inheritdoc/>
    [Pure]
    public override int GetHashCode() => Id;

    /// <inheritdoc/>
    [Pure]
    public override string ToString() => this switch
    {
        _ when this == Neutral => nameof(Neutral),
        _ when this == Unknown => nameof(Unknown),
        _ => $"P{Id}",
    };

    /// <summary>Returns true if the two <see cref="Player"/>'s are equal.</summary>
    public static bool operator ==(Player l, Player r) => l.Equals(r);

    /// <summary>Returns true if the two <see cref="Player"/>'s are not equal.</summary>
    public static bool operator !=(Player l, Player r) => !(l == r);

    /// <summary>Casts a <see cref="Player"/> to a <see cref="byte"/>.</summary>
    public static explicit operator byte(Player player) => player.Id;

    /// <summary>Parses the player.</summary>
    [Pure]
    public static Player Parse(string str)
    {
        if (string.IsNullOrEmpty(str) || nameof(Neutral).Equals(str, StringComparison.InvariantCultureIgnoreCase))
        {
            return Neutral;
        }
        else
        {
            var num = str.StartsWith("P", StringComparison.InvariantCultureIgnoreCase) ? str[1..] : str;
            return byte.TryParse(num, out var id) ? new(id) : Unknown;
        }
    }

    /// <summary>Serializes the <see cref="Player"/> as JSON string.</summary>
    [Pure]
    public string ToJson() => ToString();

    /// <summary>Deserializes the <see cref="Player"/> from a JSON number.</summary>
    [Pure]
    public static Player FromJson(long json) => new((byte)json);

    /// <summary>Deserializes the <see cref="Player"/> from a JSON string.</summary>
    [Pure]
    public static Player FromJson(string json) => Parse(json);
}
