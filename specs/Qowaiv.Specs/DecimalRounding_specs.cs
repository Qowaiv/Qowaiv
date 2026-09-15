namespace DecimalRounding_specs;

public class DirectRounding
{
    [TestCase(DecimalRounding.Truncate)]
    [TestCase(DecimalRounding.DirectAwayFromZero)]
    [TestCase(DecimalRounding.DirectTowardsZero)]
    [TestCase(DecimalRounding.Ceiling)]
    [TestCase(DecimalRounding.Floor)]
    public void Is(DecimalRounding rounding) => rounding.IsDirectRounding().Should().BeTrue();

    [TestCase(DecimalRounding.ToEven)]
    [TestCase(DecimalRounding.AwayFromZero)]
    [TestCase(DecimalRounding.ToOdd)]
    [TestCase(DecimalRounding.TowardsZero)]
    [TestCase(DecimalRounding.Up)]
    [TestCase(DecimalRounding.Down)]
    [TestCase(DecimalRounding.RandomTieBreaking)]
    [TestCase(DecimalRounding.StochasticRounding)]
    public void Not(DecimalRounding rounding) => rounding.IsDirectRounding().Should().BeFalse();
}

public class NearestRounding
{
    [TestCase(DecimalRounding.ToEven)]
    [TestCase(DecimalRounding.AwayFromZero)]
    [TestCase(DecimalRounding.ToOdd)]
    [TestCase(DecimalRounding.TowardsZero)]
    [TestCase(DecimalRounding.Up)]
    [TestCase(DecimalRounding.Down)]
    [TestCase(DecimalRounding.RandomTieBreaking)]
    [TestCase(DecimalRounding.StochasticRounding)]
    public void Is(DecimalRounding rounding) => rounding.IsNearestRounding().Should().BeTrue();

    [TestCase(DecimalRounding.Truncate)]
    [TestCase(DecimalRounding.DirectAwayFromZero)]
    [TestCase(DecimalRounding.DirectTowardsZero)]
    [TestCase(DecimalRounding.Ceiling)]
    [TestCase(DecimalRounding.Floor)]
    public void Not(DecimalRounding rounding) => rounding.IsNearestRounding().Should().BeFalse();
}
