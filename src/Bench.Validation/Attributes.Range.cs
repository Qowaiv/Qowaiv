using Qowaiv.Diagnostics.Contracts;
using Qowaiv.Financial;
using Qowaiv.Validation.DataAnnotations.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Benchmarks;

public partial class Attributes
{
    [Inheritable]
    public class Attributes_Range
    {
        private readonly RangeAttribute NonGeneric = new(typeof(Amount), "14", "42");
        private readonly InRangeAttribute<Amount> WithGenerics = new(14.0, 42.0);
        private readonly decimal[] Values = [.. Enumerable.Range(0, 1000).Select(Rnd)];

        [Benchmark(Baseline = true)]
        public bool System_ComponentModel()
        {
            var result = false;
            foreach (var value in Values)
            {
                result |= NonGeneric.IsValid(value);
            }
            return result;
        }

        [Benchmark]
        public bool generic()
        {
            var result = false;
            foreach (var country in Values)
            {
                result |= WithGenerics.IsValid(country);
            }
            return result;
        }

        private static decimal Rnd(int _) => (decimal)(Random.Shared.NextDouble() * 60d);
    }
}
