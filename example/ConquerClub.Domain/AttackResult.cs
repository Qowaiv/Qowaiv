namespace ConquerClub.Domain;

/// <summary>Represents the outcome of an attack.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
public readonly struct AttackResult(Army attacker, Army defender)
{
    /// <summary>The surviving attackers.</summary>
    public Army Attacker { get; } = attacker;

    /// <summary>The surviving defenders.</summary>
    public Army Defender { get; } = defender;

    /// <summary>The attack was successful (all defenders where killed).</summary>
    public bool IsSuccess => Defender == Army.None;

    /// <inheritdoc/>
    [Pure]
    public override string ToString() => $"{Attacker}:{Defender}";

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"Attacker: {Attacker}, Defender: {Defender}";

    /// <summary>Parses the <see cref="string"/> representing the army.</summary>
    [Pure]
    public static AttackResult Parse(string str)
    {
        if (string.IsNullOrEmpty(str)) return default;
        else
        {
            var splitted = str.Split(':');
            return splitted.Length == 2
                ? new AttackResult(Army.Parse(splitted[0]), Army.Parse(splitted[1]))
                : throw new FormatException();
        }
    }

    [Pure]
    public static AttackResult FromJson(string str) => Parse(str);
}
