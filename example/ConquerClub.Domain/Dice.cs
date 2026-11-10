namespace ConquerClub.Domain;

/// <summary>Represents the (default) dice based attack.</summary>
public static class Dice
{
    /// <summary>Attack by rolling once.</summary>
    [Pure]
    public static AttackResult Attack(Army attacker, Army defender, RandomSource rnd) =>
        new AttackResult(attacker, defender).Roll(rnd);

    /// <summary>Attack if the attacker has at least 4 left and defender has not been defeated.</summary>
    [Pure]
    public static AttackResult AutoAttack(Army attacker, Army defender, RandomSource rnd)
    {
        var combat = new AttackResult(attacker, defender);

        while (combat.Attacker >= 4 && combat.Defender >= 1)
        {
            combat = combat.Roll(rnd);
        }
        return combat;
    }

    [Pure]
    private static AttackResult Roll(this AttackResult combat, RandomSource rnd) 
        => RollDelta(combat, rnd) is { } roll
        ? roll switch
        {
            +2 => new AttackResult(combat.Attacker - 0, combat.Defender - 2),
            +1 => new AttackResult(combat.Attacker - 0, combat.Defender - 1),
            -2 => new AttackResult(combat.Attacker - 2, combat.Defender - 0),
            -1 => new AttackResult(combat.Attacker - 1, combat.Defender - 0),
            _ => new AttackResult(combat.Attacker - 1, combat.Defender - 1),
        }
        : combat;

    [Pure]
    private static int? RollDelta(AttackResult combat, RandomSource rnd) => combat.Attacker switch
    {
        var a when a >= 4 => combat.Defender >= 2 ? Roll3v2(rnd) : Roll3v1(rnd),
        var a when a >= 3 => combat.Defender >= 2 ? Roll2v2(rnd) : Roll2v1(rnd),
        var a when a >= 2 => combat.Defender >= 2 ? Roll1v2(rnd) : Roll1v1(rnd),
        _ => null,
    };

    /// <remarks>+37,2% =33,6% -29,3% E: 54,0%</remarks>
    [Pure]
    private static int Roll3v2(RandomSource rnd) => rnd.Next(2890 + 2611 + 2275) switch
    {
        var t when t < 2890 => +2,
        var t when t < 2890 + 2611 => 0,
        _ => -2,
    };

    /// <remarks>+66,0% -34,0%</remarks>
    [Pure]
    private static int Roll3v1(RandomSource rnd)
        => rnd.Next(855 + 441) < 855 ? +1 : -1;

    /// <remarks>+22,8% =32,4% -44,8% E: 39,0%</remarks>
    [Pure]
    private static int Roll2v2(RandomSource rnd) => rnd.Next(295 + 420 + 581) switch
    {
        var t when t < 295 => +2,
        var t when t < 295 + 420 => 0,
        _ => -2,
    };

    /// <remarks>+57,9% -42,1%</remarks>
    [Pure]
    private static int Roll2v1(RandomSource rnd)
        => rnd.Next(125 + 91) < 125 ? +1 : -1;

    /// <remarks>+25,5% -74,5%</remarks>
    [Pure]
    private static int Roll1v2(RandomSource rnd)
        => rnd.Next(55 + 161) < 55 ? +1 : -1;

    /// <remarks>+41,7% -58,3%</remarks>
    [Pure]
    private static int Roll1v1(RandomSource rnd)
        => rnd.Next(15 + 21) < 15 ? +1 : -1;
}
