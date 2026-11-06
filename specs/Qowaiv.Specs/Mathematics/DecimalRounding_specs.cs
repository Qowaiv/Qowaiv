namespace Mathematics.DecimalRounding_specs;

public class DirectRounding
{
    [Test]
    [Arguments(DecimalRounding.Truncate)]
    [Arguments(DecimalRounding.DirectAwayFromZero)]
    [Arguments(DecimalRounding.DirectTowardsZero)]
    [Arguments(DecimalRounding.Ceiling)]
    [Arguments(DecimalRounding.Floor)]
    public void Is(DecimalRounding rounding) => rounding.IsDirectRounding().Should().BeTrue();

    [Test]
    [Arguments(DecimalRounding.ToEven)]
    [Arguments(DecimalRounding.AwayFromZero)]
    [Arguments(DecimalRounding.ToOdd)]
    [Arguments(DecimalRounding.TowardsZero)]
    [Arguments(DecimalRounding.Up)]
    [Arguments(DecimalRounding.Down)]
    [Arguments(DecimalRounding.RandomTieBreaking)]
    [Arguments(DecimalRounding.StochasticRounding)]
    public void Not(DecimalRounding rounding) => rounding.IsDirectRounding().Should().BeFalse();
}

public class NearestRounding
{
    [Test]
    [Arguments(DecimalRounding.ToEven)]
    [Arguments(DecimalRounding.AwayFromZero)]
    [Arguments(DecimalRounding.ToOdd)]
    [Arguments(DecimalRounding.TowardsZero)]
    [Arguments(DecimalRounding.Up)]
    [Arguments(DecimalRounding.Down)]
    [Arguments(DecimalRounding.RandomTieBreaking)]
    public void Is(DecimalRounding rounding) => rounding.IsNearestRounding().Should().BeTrue();

    [Test]
    [Arguments(DecimalRounding.Truncate)]
    [Arguments(DecimalRounding.DirectAwayFromZero)]
    [Arguments(DecimalRounding.DirectTowardsZero)]
    [Arguments(DecimalRounding.Ceiling)]
    [Arguments(DecimalRounding.Floor)]
    [Arguments(DecimalRounding.StochasticRounding)]
    public void Not(DecimalRounding rounding) => rounding.IsNearestRounding().Should().BeFalse();
}
