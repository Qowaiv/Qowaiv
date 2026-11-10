using Qowaiv.Validation.Guarding;
using Qowaiv.Validation.Messages;

namespace Must_specs;

public class HaveVersion
{
    [Test]
    public void guards_expected_version()
    {
        var aggregate = new SimpleEventSourcedAggregate();
        var result = aggregate.Must().HaveVersion(0);

        result.Should().BeValid().WithoutMessages();
    }

    [Test]
    public void raises_concurrency_issue()
    {
        var aggregate = new SimpleEventSourcedAggregate();
        var result = aggregate.Must().HaveVersion(1);

        result.Should().BeInvalid().WithMessage(ConcurrencyIssue.VersionMismatch(1, 0));
    }
}
