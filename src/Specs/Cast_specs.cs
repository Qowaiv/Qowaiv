namespace Cast_specs;

public class Invalid
{
    [Test]
    [Arguments(double.MaxValue)]
    [Arguments(double.MinValue)]
    [Arguments(double.NaN)]
    [Arguments(double.NegativeInfinity)]
    [Arguments(double.PositiveInfinity)]
    public void double_to_Percentage(double num)
        => num.Invoking(n => (Percentage)n).Should().Throw<InvalidCastException>().WithMessage($"Cast from * to {typeof(Percentage)} is not valid.");

    [Test]
    [Arguments(double.MaxValue)]
    [Arguments(double.MinValue)]
    [Arguments(double.NaN)]
    [Arguments(double.NegativeInfinity)]
    [Arguments(double.PositiveInfinity)]
    public void double_to_Amount(double num)
        => num.Invoking(n => (Amount)n).Should().Throw<InvalidCastException>().WithMessage($"Cast from * to {typeof(Amount)} is not valid.");

    [Test]
    [Arguments(int.MaxValue)]
    [Arguments(1_000_000_000)]
    public void int_to_HouseNumber(int num)
        => num.Invoking(n => (HouseNumber)n).Should().Throw<InvalidCastException>().WithMessage($"Cast from * to {typeof(HouseNumber)} is not valid.");

    [Test]
    [Arguments(int.MaxValue)]
    [Arguments(1_000_000)]
    [Arguments(-1)]
    [Arguments(13)]
    public void int_to_Month(int num)
        => num.Invoking(n => (Month)n).Should().Throw<InvalidCastException>().WithMessage($"Cast from * to {typeof(Month)} is not valid.");

    [Test]
    [Arguments(int.MaxValue)]
    [Arguments(1_000_000)]
    [Arguments(-1)]
    [Arguments(10_000)]
    public void int_to_Year(int num)
        => num.Invoking(n => (Year)n).Should().Throw<InvalidCastException>().WithMessage($"Cast from * to {typeof(Year)} is not valid.");
}
