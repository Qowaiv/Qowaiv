using Qowaiv.Validation.Abstractions;
using static Specs.Abstractions.Arrange;

namespace Abstractions.Results_specs;

public class Combines
{
    [Test]
    public void Synchronious()
    {
        var f = Result.WithMessages(Error1, Warning1);
        var s = Result.WithMessages(Error2, Warning2);

        var combined = Results.Combine([f, s]);

        combined.Should().BeInvalid()
            .WithMessages(Error1, Warning1, Error2, Warning2);
    }

    [Test]
    public async Task Asynchronious()
    {
        var f = Result.WithMessages(Error1, Warning1).Async();
        var s = Result.WithMessages(Error2, Warning2).Async();

        var combined = await Results.Combine([f, s]);

        combined.Should().BeInvalid()
            .WithMessages(Error1, Warning1, Error2, Warning2);
    }
}
