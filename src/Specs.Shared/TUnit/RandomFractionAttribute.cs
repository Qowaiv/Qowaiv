using Qowaiv.Humanizer;
using Qowaiv.Mathematics;
using Specs;

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
        for (var i = 0; i < count; i++) yield return Next;
    }

    private object?[] Next()
    {
        var num = Rnd.Shared.Next(-255, 255) * Rnd.Shared.Next(255);
        
        while (num == 0 && !includeZero)
            num = Rnd.Shared.Next(-255, 255) * Rnd.Shared.Next(255);

        var den = Rnd.Shared.Next(1, 255) * Rnd.Shared.Next(1, 255);

        return [num.DividedBy(den)];
    }
}
