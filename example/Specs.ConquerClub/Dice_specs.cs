using System.Text;

namespace Dice_specs;

public class Odds
{
    [Test]
    public void For_3_vs_2_are_2890_wins_2611_draws_2275_losses()
    {
        var outcome = TwoDice([.. Get2Out3()], [.. Get2Out2()]);
        outcome.Should().Be(new DiceOutcome(2890, 2611, 2275));

        GenerateTwo(outcome, "3v2");
    }

    [Test]
    public void For_3_vs_1_are_855_wins_441_losses()
    {
        var outcome = OneDice([.. Get2Out3()], [.. Get1Out1()]);
        outcome.Should().Be(new DiceOutcome(855, 0, 441));

        GenerateOne(outcome, "3v1");
    }

    [Test]
    public void For_2_vs_2_are_295_wins_420_draws_581_losses()
    {
        var outcome = TwoDice([.. Get2Out2()], [.. Get2Out2()]);
        outcome.Should().Be(new DiceOutcome(295, 420, 581));

        GenerateTwo(outcome, "2v2");
    }

    [Test]
    public void For_2_vs_1_are_125_wins_91_losses()
    {
        var outcome = OneDice([.. Get2Out2()], [.. Get1Out1()]);
        outcome.Should().Be(new DiceOutcome(125, 0, 91));

        GenerateOne(outcome, "2v1");
    }
    [Test]
    public void For_1_vs_2_are_55_wins_161_losses()
    {
        var outcome = OneDice([.. Get1Out1()], [.. Get2Out2()]);
        outcome.Should().Be(new DiceOutcome(55, 0, 161));

        GenerateOne(outcome, "1v2");
    }

    [Test]
    public void For_1_vs_1_are_15_wins_21_losses()
    {
        var outcome = OneDice([.. Get1Out1()], [.. Get1Out1()]);
        outcome.Should().Be(new DiceOutcome(15, 0, 21));

        GenerateOne(outcome, "1v1");
    }

    private static IEnumerable<int> Roll() => Enumerable.Range(1, 6).Reverse();

    private static IEnumerable<Distribution> Get2Out3()
    {
        var lookup = new Dictionary<DicePair, int>();

        foreach (var a in Roll())
        {
            foreach (var b in Roll())
            {
                foreach (var c in Roll())
                {
                    var all = (new[] { a, b, c }).OrderByDescending(i => i).Take(2).ToArray();
                    var dice = new DicePair(all[0], all[1]);

                    lookup[dice] = lookup.TryGetValue(dice, out var count)
                        ? count + 1
                        : 1;
                }
            }
        }
        foreach (var kvp in lookup)
        {
            yield return new Distribution(kvp.Key, kvp.Value);
        }
    }
    private static IEnumerable<Distribution> Get2Out2()
    {
        var lookup = new Dictionary<DicePair, int>();

        foreach (var a in Roll())
        {
            foreach (var b in Roll())
            {
                var all = (new[] { a, b }).OrderByDescending(i => i).ToArray();
                var dice = new DicePair(all[0], all[1]);

                lookup[dice] = lookup.TryGetValue(dice, out int count) ? ++count : 1;
            }
        }
        foreach (var kvp in lookup)
        {
            yield return new Distribution(kvp.Key, kvp.Value);
        }
    }
    private static IEnumerable<Distribution> Get1Out1()
        => Roll().Select(d => new Distribution(new DicePair(d, 0), 1));

    private static DiceOutcome TwoDice(Distribution[] attackers, Distribution[] defenders)
    {
        var w = 0;
        var d = 0;
        var l = 0;

        foreach (var attack in attackers)
        {
            foreach (var defender in defenders)
            {
                var weight = attack.Frequency * defender.Frequency;
                var score = 0;

                if (attack.Dice.Hi > defender.Dice.Hi)
                {
                    score++;
                }
                else
                {
                    score--;
                }
                if (attack.Dice.Lo > defender.Dice.Lo)
                {
                    score++;
                }
                else
                {
                    score--;
                }
                if (score == 0)
                {
                    d += weight;
                }
                else if (score > 0)
                {
                    w += weight;
                }
                else
                {
                    l += weight;
                }
            }
        }
        return new DiceOutcome(w, d, l);
    }

    private static DiceOutcome OneDice(Distribution[] attackers, Distribution[] defenders)
    {
        var w = 0;
        var d = 0;
        var l = 0;

        foreach (var attack in attackers)
        {
            foreach (var defender in defenders)
            {
                var weight = attack.Frequency * defender.Frequency;
                if (attack.Dice.Hi > defender.Dice.Hi)
                {
                    w += weight;
                }
                else
                {
                    l += weight;
                }
            }
        }
        return new DiceOutcome(w, d, l);
    }

    private static void GenerateTwo(DiceOutcome outcome, string suffix)
    {
        var sb = new StringBuilder();
        sb
            .AppendLine($"/// <remarks>+{outcome.Win / (decimal)outcome.Total:0.0%} ={outcome.Draw / (decimal)outcome.Total:0.0%} -{outcome.Loss / (decimal)outcome.Total:0.0%} E: {(outcome.Win + outcome.Draw / 2m) / outcome.Total:0.0%}</remarks>")
            .AppendLine($"private static int Roll{suffix}(IGenerator rnd)")
            .AppendLine(@"{")
            .AppendLine($"    var threshold = rnd.Next({outcome.Win} + {outcome.Draw} + {outcome.Loss});")
            .AppendLine($"    if (threshold < {outcome.Win}) {{ return +2; }}")
            .AppendLine($"    if (threshold < {outcome.Win} + {outcome.Draw}) {{ return 0; }}")
            .AppendLine($"    else {{ return -2; }}")
            .AppendLine(@"}")
        ;
        Console.WriteLine(sb);
    }

    private static void GenerateOne(DiceOutcome outcome, string suffix)
    {
        var sb = new StringBuilder();
        sb
            .AppendLine($"/// <remarks>+{outcome.Win / (decimal)outcome.Total:0.0%} -{outcome.Loss / (decimal)outcome.Total:0.0%}</remarks>")
            .AppendLine($"private static int Roll{suffix}(IGenerator rnd)")
            .AppendLine(@"{")
            .AppendLine($"    return rnd.Next({outcome.Win} + {outcome.Loss}) < {outcome.Win} ? +1 : -1;")
            .AppendLine(@"}")
        ;
        Console.WriteLine(sb);
    }

    private readonly struct DiceOutcome(int win, int draw, int loss)
    {
        public int Win { get; } = win;
        public int Draw { get; } = draw;
        public int Loss { get; } = loss;
        public int Total => Win + Draw + Loss;

        public override string ToString() => $"+{Win} ={Draw} -{Loss}";
    }

    internal readonly struct Distribution(DicePair dice, int frequency)
    {
        public DicePair Dice { get; } = dice;

        public int Frequency { get; } = frequency;

        public override string ToString() => $"{Dice}: {Frequency}";
    }

    internal readonly record struct DicePair(int Hi, int Lo)
    {
        public override string ToString() => $"{Hi}{Lo}";
    }
}

public class AutoAttack
{
    [Test]
    public void Defender_is_None_on_successful_attack()
    {
        var rnd = new MersenneTwister(14);
        var attacker = Player.P1.Army(10);
        var defender = Player.P2.Army(5);

        var result = Dice.AutoAttack(attacker, defender, rnd);

        result.Should().Be(new AttackResult(Player.P1.Army(7), Army.None));
    }

    [Test]
    public void Defender_has_armies_on_unsuccessful_attack()
    {
        var rnd = new MersenneTwister(24);
        var attacker = Player.P1.Army(10);
        var defender = Player.P2.Army(8);

        var result = Dice.AutoAttack(attacker, defender, rnd);

        result.IsSuccess.Should().BeFalse();
        result.Should().Be(new AttackResult(Player.P1.Army(3), Player.P2.Army(3)));
    }
}
