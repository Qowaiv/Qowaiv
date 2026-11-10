using Qowaiv.Extensions;
using Specs;

namespace Qowaiv.Specs.TestTools;

/// <summary>Generates random values between <c>min</c> and <c>max</c> as arguments for a (numeric) test parameter.</summary>
/// <remarks>
/// Requires the test method to be decorated with <see cref="CombinedDataSourcesAttribute" />.
/// </remarks>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public sealed class RandomAttribute(double min, double max, int count) : UntypedDataSourceGeneratorAttribute
{
    public RandomAttribute(int min, int max, int count) : this((double)min, max, count) { }

    /// <inheritdoc />
    [Pure]
    protected override IEnumerable<Func<object?[]?>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        for (var i = 0; i < count; i++) yield return Next;
    }

    private object?[] Next() => [(min + (Rnd.Shared.NextDouble() * (max - min))).ToDecimal()];
}
