namespace Collection_extensions_specs;

public class Amounts
{
    private static readonly Amount[] collection = new[] { (Amount)1, (Amount)23, (Amount)0 };
    private static readonly Amount?[] nullables = new Amount?[] { (Amount)1, (Amount)23, (Amount)0, null };

    [Test]
    public void Average_on_collection_is_caluculated()
        => collection.Average().Should().Be(8.Amount());

    [Test]
    public void Average_on_nullables_can_be_calulated()
        => nullables.Average().Should().Be(8.Amount());

    [Test]
    public void Average_on_empty_collection_throws()
    {
        Func<Amount> average = () => Array.Empty<Amount>().Average();
        average.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void Average_on_nullable_empty_collection_is_zero()
        => Array.Empty<Amount?>().Average().Should().BeNull();

    [Test]
    public void Sum_on_collection_is_caluculated()
       => collection.Sum().Should().Be(24.Amount());

    [Test]
    public void Sum_on_nullables_can_be_calulated()
        => nullables.Sum().Should().Be(24.Amount());

    [Test]
    public void Sum_on_empty_collection_is_zero()
        => Array.Empty<Amount>().Sum().Should().Be(Amount.Zero);

    [Test]
    public void Sum_on_nullable_empty_collection_is_zero()
        => Array.Empty<Amount?>().Sum().Should().BeNull();
}

public class Moneys
{
    private static readonly Money[] collection = new[] { 1 + Currency.EUR, 23 + Currency.EUR, 0 + Currency.EUR };
    private static readonly Money?[] nullables = new Money?[] { 1 + Currency.EUR, 23 + Currency.EUR, 0 + Currency.EUR, null };

    private static readonly Money[] mixed = new[] { 1 + Currency.EUR, 23 + Currency.USD };
    private static readonly Money?[] mixedNulables = new Money?[] { 1 + Currency.EUR, 23 + Currency.USD };

    [Test]
    public void Average_on_collection_is_caluculated()
        => collection.Average().Should().Be(8 + Currency.EUR);

    [Test]
    public void Average_on_nullables_can_be_calulated()
        => nullables.Average().Should().Be(8 + Currency.EUR);

    [Test]
    public void Average_on_empty_collection_throws()
    {
        Func<Money> average = () => Array.Empty<Money>().Average();
        average.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void Average_on_nullable_empty_collection_is_zero()
        => Array.Empty<Money?>().Average().Should().BeNull();

    [Test]
    public void Average_on_mixed_currencies_throws()
    {
        Func<Money> average = () => mixed.Average();
        average.Should().Throw<CurrencyMismatchException>();
    }

    [Test]
    public void Average_on_mixed_nullables_currencies_throws()
    {
        Func<Money?> average = () => mixedNulables.Average();
        average.Should().Throw<CurrencyMismatchException>();
    }

    [Test]
    public void Sum_on_collection_is_caluculated()
        => collection.Sum().Should().Be(24 + Currency.EUR);

    [Test]
    public void Sum_on_nullables_can_be_calulated()
        => nullables.Sum().Should().Be(24 + Currency.EUR);

    [Test]
    public void Sum_on_empty_collection_throws()
        => Array.Empty<Money>().Sum().Should().Be(Money.Zero);

    [Test]
    public void Sum_on_nullable_empty_collection_is_zero()
        => Array.Empty<Money?>().Sum().Should().BeNull();

    [Test]
    public void Sum_on_mixed_currencies_throws()
    {
        Func<Money> average = () => mixed.Sum();
        average.Should().Throw<CurrencyMismatchException>();
    }

    [Test]
    public void Sum_on_mixed_nullables_currencies_throws()
    {
        Func<Money?> average = () => mixedNulables.Sum();
        average.Should().Throw<CurrencyMismatchException>();
    }
}
