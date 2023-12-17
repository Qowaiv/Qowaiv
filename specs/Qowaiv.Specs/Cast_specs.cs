namespace Cast_specs;

public class Invalid
{
    [TestCase(double.MaxValue)]
    [TestCase(double.MinValue)]
    [TestCase(double.NaN)]
    [TestCase(double.NegativeInfinity)]
    [TestCase(double.PositiveInfinity)]
    public void double_to_Percentage(double num)
        => num.Invoking(n => (Percentage)n).Should().Throw<InvalidCastException>().WithMessage($"Cast from .+ to {typeof(Percentage)} is not valid.");

    [TestCase(double.MaxValue)]
    [TestCase(double.MinValue)]
    [TestCase(double.NaN)]
    [TestCase(double.NegativeInfinity)]
    [TestCase(double.PositiveInfinity)]
    public void double_to_Amount(double num)
        => num.Invoking(n => (Amount)n).Should().Throw<InvalidCastException>().WithMessage($"Cast from .+ to {typeof(Amount)} is not valid.");

    [TestCase(int.MaxValue)]
    [TestCase(1_000_000_000)]
    public void int_to_HouseNumber(int num)
        => num.Invoking(n => (HouseNumber)n).Should().Throw<InvalidCastException>().WithMessage($"Cast from .+ to {typeof(HouseNumber)} is not valid.");

    [TestCase(int.MaxValue)]
    [TestCase(1_000_000)]
    [TestCase(-1)]
    [TestCase(13)]
    public void int_to_Month(int num)
        => num.Invoking(n => (Month)n).Should().Throw<InvalidCastException>().WithMessage($"Cast from .+ to {typeof(Month)} is not valid.");

    [TestCase(int.MaxValue)]
    [TestCase(1_000_000)]
    [TestCase(-1)]
    [TestCase(10_000)]
    public void int_to_Year(int num)
        => num.Invoking(n => (Year)n).Should().Throw<InvalidCastException>().WithMessage($"Cast from .+ to {typeof(Year)} is not valid.");
}
