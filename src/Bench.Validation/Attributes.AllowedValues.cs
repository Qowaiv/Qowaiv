using Qowaiv.Diagnostics.Contracts;
using Qowaiv.Globalization;
using Qowaiv.Validation.DataAnnotations;

namespace Benchmarks;

public partial class Attributes
{
    [Inheritable]
    public class Attributes_AllowedValues
    {
        private readonly AllowedValuesAttribute NonGeneric = new("NL", "BE", "LU", "DE", "FR");
        private readonly AllowedAttribute<Country> WithGenerics = new("NL", "BE", "LU", "DE", "FR");

        [Benchmark(Baseline = true)]
        public bool non_generic()
        {
            var result = false;
            foreach (var country in Country.All)
            {
                result |= NonGeneric.IsValid(country);
            }
            return result;
        }

        [Benchmark]
        public bool generic()
        {
            var result = false;
            foreach (var country in Country.All)
            {
                result |= WithGenerics.IsValid(country);
            }
            return result;
        }
    }
}
