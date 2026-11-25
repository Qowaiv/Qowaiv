using Qowaiv.Extensions;
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
        while(true)
        {
            var num = Rnd.Shared.NextFraction(-Fraction.One, +Fraction.One, 256);
            if (!num.IsZero || includeZero) return [num];
        }
    }
}
