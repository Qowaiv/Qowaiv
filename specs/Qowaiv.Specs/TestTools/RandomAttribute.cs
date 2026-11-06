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
        var type = ((ParameterMetadata)dataGeneratorMetadata.MembersToGenerate[0]).Type;
        var rnd = new Random();

        for (var i = 0; i < count; i++)
        {
            var value = Convert(min + (rnd.NextDouble() * (max - min)), type);
            yield return () => [value];
        }
    }

    [Pure]
    private static object Convert(double value, Type type)
        => type == typeof(decimal)
        ? ToDecimal(value)
        : System.Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

    [Pure]
    private static decimal ToDecimal(double value)
        => value >= (double)decimal.MaxValue ? decimal.MaxValue
        : value <= (double)decimal.MinValue ? decimal.MinValue
        : (decimal)value;
}