namespace Qowaiv.Specs.TestTools;

/// <summary>Generates random <see cref="Fraction" />s as arguments for a test parameter.</summary>
/// <remarks>
/// Requires the test method to be decorated with <see cref="CombinedDataSourcesAttribute" />.
/// </remarks>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public sealed class RandomFractionAttribute(int count, bool includeZero = false) : UntypedDataSourceGeneratorAttribute
{
    /// <inheritdoc />
    [Pure]
    protected override IEnumerable<Func<object?[]?>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        var rnd = new Random();

        for (var i = 0; i < count; i++)
        {
            var fraction = Fraction(rnd);
            yield return () => [fraction];
        }
    }

    [Pure]
    private Fraction Fraction(Random rnd)
    {
        var num = rnd.Next(-255, 255) * rnd.Next(255);
        while (num == 0 && !includeZero)
        {
            num = rnd.Next(-255, 255) * rnd.Next(255);
        }
        var den = rnd.Next(1, 255) * rnd.Next(1, 255);
        return num.DividedBy(den);
    }
}