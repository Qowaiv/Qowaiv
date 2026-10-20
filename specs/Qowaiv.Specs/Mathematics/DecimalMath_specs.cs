namespace Mathematics.DecimalMath_specs;

public class Constants
{
    [Test]
    public void Epsilon() => DecimalMath.Epsilon.Should().Be(new decimal(1, 0, 0, isNegative: false, scale: 28));

    [Test]
    public void E() => DecimalMath.E.Should().Be(2.7182818284590452353602874714m);

    [Test]
    public void Pi() => DecimalMath.Pi.Should().Be(3.1415926535897932384626433833m);
}

public class Rounds
{

}

public class Round_to_multple
{

}
