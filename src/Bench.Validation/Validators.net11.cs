using Qowaiv.Validation.Abstractions;
#if NET11_0_OR_GREATER
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Validation;
using Qowaiv.Validation.TestData;

namespace Benchmarks;

public partial class Validators
{
    public partial class Validate_small
    {
        private static readonly ValidationOptions MicrosoftOptions = CreateMicrosoftOptions();
        private static readonly Qowaiv.Validation.Microsoft.ModelValidator<ModelWrapper> MicrosoftValidator = new(MicrosoftOptions);

        [Benchmark]
        public Result Microsoft() => MicrosoftValidator.Validate(new ModelWrapper(Model));

        private static ValidationOptions CreateMicrosoftOptions()
        {
            var services = new ServiceCollection();
            services.AddValidation();
            using var provider = services.BuildServiceProvider();
            return provider.GetRequiredService<IOptions<ValidationOptions>>().Value;
        }

        [ValidatableType]
        public sealed record ModelWrapper(BenchmarkModel Model);
    }
}
#else
namespace Benchmarks;

public partial class Validators
{
    public partial class Validate_small
    {
        [Benchmark]
        public Result Microsoft() => throw new NotSupportedException();
    }
}
#endif
