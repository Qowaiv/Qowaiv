using Qowaiv.Validation.DataAnnotations;
using Qowaiv.Validation.TestData;

namespace Specs.Benchmarks.Data_Annotations;

public class Validate_is_valid_for
{
    private readonly BenchmarkModel Model = BenchmarkModel.New(1000);

    [Test]
    public void Qowaiv_AnnotatedModelValidator()
        => AnnotatedModelValidator.Validate(Model).Should().BeValid().WithoutMessages();

    [Test]
    public void FluentValidation_AbstractValidator()
        => new BenchmarkModelValidator().Validate(Model).IsValid.Should().BeTrue();

    [Test]
    public void MiniValidator()
        => MiniValidation.MiniValidator.TryValidate(Model, out var _).Should().BeTrue();
}
